// <copyright file="UsersController.cs" company="BuFaKWiSo">
// Copyright (c) BuFaKWiSo. All rights reserved.
// </copyright>

namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using BuFaKAPI.Models.SubModels;
    using BuFaKAPI.Services;
    using Firebase.Auth;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using WebApplication1.Models;
    using User = WebApplication1.Models.User;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly string apikey;
        private readonly TokenService jwtService;
        private readonly TelegramBot telBot;
        private readonly AuthService auth;
        private readonly FirebaseService firebase;

        public UsersController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.apikey = settings.Value.FirebaseApiKey;
            this.jwtService = new TokenService(this._context, settings);
            this.telBot = new TelegramBot();
            this.auth = new AuthService(context);
            this.firebase = new FirebaseService(context, settings);
        }

        // GET: api/Users/byconference/1

        /// <summary>
        /// Gets all Users from a specific conference
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>List of Users (UID, Surname, Name)</returns>
        [HttpGet("byconference/{conference_id}")]
        public IActionResult GetUser([FromRoute] int conference_id, [FromQuery] string apikey)
        {
            // TODO Permission Level Admin
            List<UserByConference> ubc = new List<UserByConference>();
            if (this.auth.KeyIsValid(apikey, conference_id))
            {
                var applications = this._context.Conference_Application.Where(ca => ca.ConferenceID == conference_id);
                foreach (Conference_Application ca in applications)
                {
                    var user = this._context.User.Where(u => u.UID == ca.ApplicantUID).FirstOrDefault();
                    UserByConference uc = new UserByConference
                    {
                        UID = user.UID,
                        Surname = user.Surname,
                        Name = user.Name
                    };
                    ubc.Add(uc);
                }
            }

            return this.Ok(ubc);
        }

        // GET: api/Users/5

        /// <summary>
        /// Gets one specific User of the Database
        /// </summary>
        /// <param name="id">ID of the User to be get</param>
        /// <param name="jwttoken">JW Token for Authentification</param>
        /// <returns>User-Object in Question on Success</returns>
        /// <response code="400">Bad Request, if ModelState is not valid</response>
        /// <response code="401">Unauthorized, if JWToken is not valid</response>
        /// <response code="404">Not Found, if User ID is not found in the Database</response>
        /// <response code="418">I'm A Teapot, if JWToken Lifespan is exceeded</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id, [FromHeader] string jwttoken)
        {
            // TODO Permission Level User
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this._context.User.FindAsync(id);

            if (user == null)
            {
                return this.NotFound();
            }

            try
            {
                if (this.jwtService.ValidateJwtKey(jwttoken))
                {
                    if (this.jwtService.GetUIDfromJwtKey(jwttoken) == id)
                    {
                        return this.Ok(user);
                    }
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return new StatusCodeResult(StatusCodes.Status418ImATeapot);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Triggers a passwort forgot mail
        /// </summary>
        /// <param name="email">Email of the User</param>
        /// <returns>No content</returns>
        [HttpPut("passwordforget/")]
        public async Task<IActionResult> PasswordForget([FromBody] Mail email)
        {
            await this.firebase.PasswordReset(email.EMail);
            return this.Ok();
        }

        /// <summary>
        /// Changes the Password of a specific User
        /// </summary>
        /// <param name="newpassword">The Data used for the Password-Change</param>
        /// <returns>No Content</returns>
        [HttpPut("passwordchange/")]
        public async Task<IActionResult> PasswordChange([FromBody] NewPasswordObject newpassword)
        {
            // TODO Permission Level User
            try
            {
                var response = await this.firebase.PasswordChange(newpassword.Email, newpassword.OldPassword, newpassword.NewPassword);
                if (string.IsNullOrWhiteSpace(response))
                {
                    return this.Conflict();
                }
                else if (response == "302")
                {
                    return new StatusCodeResult(StatusCodes.Status226IMUsed);
                }
                else
                {
                    return this.Ok(response);
                }
            }
            catch (FirebaseAuthException)
            {
                return this.BadRequest();
            }
            catch (AggregateException)
            {
                return this.BadRequest();
            }
        }

        /// <summary>
        /// Function to change the Mail Adress
        /// </summary>
        /// <param name="newemail">NewEmail Object</param>
        /// <returns>No Content</returns>
        [HttpPut("newemail/")]
        public async Task<IActionResult> NewEmail([FromBody] NewEmailObject newemail)
        {
            // TODO Permission Level User
            try
            {
                var response = await this.firebase.EmailChange(newemail.OldEmail, newemail.Password, newemail.NewEmail);
                if (string.IsNullOrWhiteSpace(response))
                {
                    return this.Conflict();
                }
                else if (response == "302")
                {
                    return new StatusCodeResult(StatusCodes.Status302Found);
                }

                var user = this._context.User.FindAsync(newemail.UID).Result;
                user.Email = newemail.NewEmail;
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.UserExists(newemail.UID))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.Ok(response);
            }
            catch (FirebaseAuthException)
            {
                return this.BadRequest();
            }
            catch (AggregateException)
            {
                return this.BadRequest();
            }
        }

        /// <summary>
        /// Updates a specific User in the Database
        /// </summary>
        /// <param name="id">ID of the User to be Updated</param>
        /// <param name="jwttoken">JW Token for authentification</param>
        /// <param name="user">User-Object to be Updated</param>
        /// <returns>204 on Success</returns>
        /// <response code="400">Bad Request, if ModelState is not valid</response>
        /// <response code="401">Unauthorized, if JW Token is not valid</response>
        /// <response code="404">Not Found, when User ID not found in the Database</response>
        /// <response code="418">I'm A Teapot, if JW Token Lifespan is exceeded</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromHeader] string jwttoken, [FromBody] User user)
        {
            // TODO Permission Level User
            try
            {
                if (this.jwtService.ValidateJwtKey(jwttoken))
                {
                    if (this.jwtService.GetUIDfromJwtKey(jwttoken) == id)
                    {
                        if (!this.ModelState.IsValid)
                        {
                            return this.BadRequest(this.ModelState);
                        }

                        if (id != user.UID)
                        {
                            return this.BadRequest();
                        }

                        this._context.Entry(user).State = EntityState.Modified;

                        try
                        {
                            await this._context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!this.UserExists(id))
                            {
                                return this.NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }

                        return this.NoContent();
                    }
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return new StatusCodeResult(StatusCodes.Status418ImATeapot);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Adds a new User to the Database, with Firebase entry
        /// </summary>
        /// <param name="userWithPassword">User Object to be parsed and added</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>User-Object on Success</returns>
        /// <response code="400">Bad Request, if ModelState is not valid</response>
        /// <response code="401">Unauthorized, if API Key is not valid</response>
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserWithPassword userWithPassword, [FromQuery] string apikey)
        {
            if (this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(this.apikey));

                try
                {
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(userWithPassword.Email, userWithPassword.Password);
                    var firebasetoken = authProvider.SignInWithEmailAndPasswordAsync(userWithPassword.Email, userWithPassword.Password).Result.FirebaseToken;
                    await this.firebase.SendVerificationEmail(firebasetoken);

                    // TODO check if usefull
                    if (auth.User == null)
                    {
                        return this.BadRequest(this.ModelState);
                    }

                    User user = new User
                    {
                        Name = userWithPassword.Name,
                        Surname = userWithPassword.Surname,
                        Email = userWithPassword.Email,
                        Birthday = userWithPassword.Birthday,
                        CouncilID = userWithPassword.Council_id,
                        UID = auth.User.LocalId,
                        Sex = userWithPassword.Sex,
                        Address = userWithPassword.Address,
                        Note = userWithPassword.Note,
                    };

                    string token = this.jwtService.CreateKey(user.UID);
                    this.telBot.SendTextMessage($"User Created, Name: {user.Surname} {user.Name}");
                    var userreturn = new
                    {
                        User = user,
                        JwtToken = token
                    };
                    this._context.User.Add(user);
                    await this._context.SaveChangesAsync();
                    return this.Ok(userreturn);

                    // return this.CreatedAtAction("GetUser", new { id = user.UID }, Tuple.Create(user, token));
                }

                // Catch Email Exists usw.
                catch (Exception)
                {
                    // { "error": { "code": 400, "message": "EMAIL_EXISTS", "errors": [ { "message": "EMAIL_EXISTS", "domain": "global", "reason": "invalid" } ] } }
                    return this.BadRequest(this.ModelState);
                }
            }

            return this.Unauthorized();
        }

        // DELETE: api/Users/5

        /// <summary>
        /// Deletes a specific User from the Database
        /// </summary>
        /// <param name="jwttoken">JWT Token for Authentification of the User</param>
        /// <param name="uwp">UserWithPassword Object needed for deletion</param>
        /// <returns>User that was deleted</returns>
        /// <response code="400">Bad Request, if ModelState is not valid</response>
        /// <response code="401">Unauthorized, if JWT Token is not valid</response>
        /// <response code="418">I'm A Teapot, if JWT Token lifetime is exceeded</response>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromHeader] string jwttoken, [FromBody] UserWithPassword uwp)
        {
            // TODO Permission Level User
            try
            {
                if (this.jwtService.ValidateJwtKey(jwttoken))
                {
                    if (this.jwtService.GetUIDfromJwtKey(jwttoken) == uwp.Uid)
                    {
                        if (!this.ModelState.IsValid)
                        {
                            return this.BadRequest(this.ModelState);
                        }

                        var user = await this._context.User.FindAsync(uwp.Uid);
                        if (user == null)
                        {
                            return this.NotFound();
                        }

                        this._context.User.Remove(user);
                        await this._context.SaveChangesAsync();
                        this.firebase.DeleteUser(uwp.Uid, uwp.Password);
                        return this.Ok(user);
                    }
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return new StatusCodeResult(StatusCodes.Status418ImATeapot);
            }

            return this.Unauthorized();
            }

        private bool UserExists(string id)
        {
            return this._context.User.Any(e => e.UID == id);
        }
    }
}