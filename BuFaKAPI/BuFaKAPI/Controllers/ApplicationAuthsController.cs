namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using WebApplication1.Models;

    public class OtherKeys
    {
        public int OtherKeysCount { get; set; }
    }

    /// <summary>
    /// Controller for checking if an Applicant is authorized to apply for a conference
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationAuthsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;
        private readonly TelegramBot telBot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationAuthsController"/> class.
        /// Constructor for the ApplicationAuthsController
        /// </summary>
        /// <param name="context">Context used for the .Net Application</param>
        /// <param name="settings">Settings</param>
        public ApplicationAuthsController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(this._context);
            this.jwtService = new TokenService(this._context, settings);
            this.telBot = new TelegramBot();
        }

        /// <summary>
        /// Checks if the User has the rights to apply for a Conference
        /// </summary>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="applicationAuth">Object to check</param>
        /// <param name="jwttoken">Token of the User for Auth</param>
        /// <param name="conference_id">Id of the Conference in Question</param>
        /// <returns>PasswordFound(Bool) and Priority(int)</returns>
        [HttpPut]
        public async Task<IActionResult> ApplicantIsAuthorized(
            [FromQuery] string apikey,
            [FromBody] ApplicationAuth applicationAuth,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromHeader(Name = "conference_id")] int conference_id)
        {
            if (this.auth.KeyIsValid(apikey) && this.jwtService.PermissionLevelValid(jwttoken, "user"))
            {
                var authForConf = this._context.ApplicationAuth.Where(a => a.Conference_ID == conference_id
                                                                        && a.Council_ID == applicationAuth.Council_ID
                                                                        && a.Password == applicationAuth.Password
                                                                        && a.Used == false).FirstOrDefault();

                if (authForConf == null)
                {
                    var authForConfOtherKey = this._context.ApplicationAuth.Where(a => a.Conference_ID == conference_id
                                                                        && a.Council_ID == 0
                                                                        && a.Password == applicationAuth.Password
                                                                        && a.Used == false).FirstOrDefault();
                    if (authForConfOtherKey == null)
                    {
                        return this.Ok(new { PasswordFound = false, Priority = 0 });
                    }

                    return this.Ok(new { PasswordFound = true, Prioriy = authForConfOtherKey.Priority, IsOtherKey = true });
                }
                else
                {
                    return this.Ok(new { PasswordFound = true, Prioriy = authForConf.Priority });
                }
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// generates passwords for the councils
        /// </summary>
        /// <param name="jwttoken">User token for Authentification</param>
        /// <param name="conference_id">ID of the current Conference</param>
        /// <param name="otherKeys">The Count of Out-of-council keys to create</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>Nothing</returns>
        [HttpPut("generate/")]
        public async Task<IActionResult> GeneratePasswordsForCouncils(
            [FromHeader(Name = "jwtkey")] string jwttoken,
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromBody] OtherKeys otherKeys,
            [FromQuery] string apikey)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                if (this._context.ApplicationAuth.Any(aa => aa.Conference_ID == conference_id))
                {
                    return this.NoContent();
                }
                else
                {
                    var councils = this._context.Council.Where(c => c.Invalid == false).ToList();

                    // List<ApplicationAuth> appAuth = new List<ApplicationAuth>();
                    foreach (Council currentCouncil in councils)
                    {
                        ApplicationAuth one = new ApplicationAuth
                        {
                            Council_ID = currentCouncil.CouncilID,
                            Conference_ID = conference_id,
                            Priority = 1,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        ApplicationAuth two = new ApplicationAuth
                        {
                            Council_ID = currentCouncil.CouncilID,
                            Conference_ID = conference_id,
                            Priority = 2,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        ApplicationAuth three = new ApplicationAuth
                        {
                            Council_ID = currentCouncil.CouncilID,
                            Conference_ID = conference_id,
                            Priority = 3,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        ApplicationAuth four = new ApplicationAuth
                        {
                            Council_ID = currentCouncil.CouncilID,
                            Conference_ID = conference_id,
                            Priority = 4,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        ApplicationAuth five = new ApplicationAuth
                        {
                            Council_ID = currentCouncil.CouncilID,
                            Conference_ID = conference_id,
                            Priority = 5,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        ApplicationAuth six = new ApplicationAuth
                        {
                            Council_ID = currentCouncil.CouncilID,
                            Conference_ID = conference_id,
                            Priority = 6,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        this._context.Add(one);
                        this._context.Add(two);
                        this._context.Add(three);
                        this._context.Add(four);
                        this._context.Add(five);
                        this._context.Add(six);
                        try
                        {
                            await this._context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            this.telBot.SendTextMessage($"Error at generating passwords for {currentCouncil.CouncilID}, {currentCouncil.Name} - {currentCouncil.University}");
                        }
                    }

                    for (int i = 0; i <= otherKeys.OtherKeysCount; i++)
                    {
                        ApplicationAuth key = new ApplicationAuth
                        {
                            Council_ID = 0,
                            Conference_ID = conference_id,
                            Priority = 1,
                            Password = this.GeneratePassword(),
                            Used = false,
                        };
                        this._context.Add(key);
                        try
                        {
                            await this._context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            this.telBot.SendTextMessage($"Error at generating otherKey");
                        }
                    }

                    return this.Ok();
                }
            }

            return this.Unauthorized();
        }

        private bool ApplicationAuthExists(int id)
        {
            return this._context.ApplicationAuth.Any(e => e.ID == id);
        }

        private string GeneratePassword()
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz|-[]";
            StringBuilder result = new StringBuilder(8);
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }
    }
}