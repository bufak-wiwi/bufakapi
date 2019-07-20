// <copyright file="LoginController.cs" company="BuFaKWiSo">
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
    using Microsoft.Extensions.Options;
    using WebApplication1.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly string apikey;
        private readonly TokenService jwtService;
        private readonly TelegramBot telBot;
        private readonly int currentConferenceId;
        private readonly FirebaseService fbService;
        private readonly AuthService auth;

        public LoginController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.apikey = settings.Value.FirebaseApiKey;
            this.currentConferenceId = settings.Value.CurrentConferenceID;
            this.jwtService = new TokenService(settings);
            this.telBot = new TelegramBot();
            this.fbService = new FirebaseService(context, settings);
            this.auth = new AuthService(context);
        }

        /// <summary>
        /// Lets the User Log in and gets a session from the API
        /// </summary>
        /// <param name="loginElement">A Login Element, meaning an E-Mail and a Password</param>
        /// <returns>A Result-Object</returns>
        /// <response code="400">If the ModelState is not valid</response>
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginElement loginElement)
        {
            // TODO Permission Level User
            var email = loginElement.Email;
            var password = loginElement.Password;
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(this.apikey));
            try
            {
                var result = new LoginResult();
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                if (!auth.IsExpired() && !string.IsNullOrEmpty(auth.User.LocalId))
                {
                    result.TokenString = this.jwtService.CreateKey(auth.User.LocalId);
                    result.user = await this._context.User.FindAsync(auth.User.LocalId);

                    // set values in result whether the user has already applied to the conference or not
                    result = this.SetAppliedStatus(auth, result);
                    result = this.SetAdminStatus(auth, result);

                    return this.Ok(result);
                }
                else
                {
                    return this.BadRequest(this.ModelState);
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(this.ModelState);
            }
        }

        [HttpGet("customToken/{uid}")]
        public IActionResult GetCustomTokens([FromRoute] string uid, [FromQuery] string apikey, [FromHeader] int conference_id)
        {
            // TODO Permission Level SuperAdmin
            if (this.auth.KeyIsValid(apikey))
            {
                if (this._context.Conference.Any(c => c.ConferenceID == conference_id))
                {
                    /*if (this._context.Conference_Attendee.Any(u => u.Attendee_uid == uid && u.ConferenceID == conference_id))
                    {
                        var startdate = this._context.Conference.Find(conference_id).DateStart;
                        var enddate = this._context.Conference.Find(conference_id).DateEnd;
                        //if (DateTime.Now >= DateTime.Parse(startdate) && DateTime.Now <= DateTime.Parse(enddate))
                        //{
                            var customKey = this.fbService.CreateCustomFBKey(uid);
                            return this.Ok(customKey);
                        //}
                        //else
                        //{
                        //    this.telBot.sendTextMessage($"Access to GetCustomTokens from {UID} not within correct Timespan");
                        //    return this.Unauthorized();
                        //}

                    }
                    else
                    {
                        this.telBot.sendTextMessage($"Access to GetCustomTokens from {uid}, user not in Conference_Attendee-Table");
                        return this.Unauthorized();
                    }*/
                }
                else
                {
                    this.telBot.SendTextMessage($"Access to GetCustomTokens from {uid}, Conference not in Database");
                    return this.NotFound();
                }
            }
            else
            {
                return this.Unauthorized();
            }

            return this.NotFound();
        }

        private LoginResult SetAppliedStatus(FirebaseAuthLink auth, LoginResult result)
        {
            var uid = auth.User.LocalId;
            if (!string.IsNullOrWhiteSpace(uid))
            {
                Conference_Application application = this._context.Conference_Application.Where(x => x.ApplicantUID == uid && x.ConferenceID == this.currentConferenceId).FirstOrDefault();
                result.Applied = application != null ? true : false;
                result.Priority = application != null ? application.Priority : 0;
            }

            return result;
        }

        private LoginResult SetAdminStatus(FirebaseAuthLink auth, LoginResult result)
        {
            var uid = auth.User.LocalId;
            if (!string.IsNullOrWhiteSpace(uid))
            {
                Administrator admin = this._context.Administrator.Where(x => x.UID == uid && x.ConferenceID == this.currentConferenceId).FirstOrDefault();
                result.Admin = admin != null ? true : false;
            }

            return result;
        }
    }
}