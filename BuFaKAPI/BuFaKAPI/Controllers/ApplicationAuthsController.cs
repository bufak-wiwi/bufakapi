namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

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
        }

        /// <summary>
        /// Checks if the User has the rights to apply for a Conference
        /// </summary>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="applicationAuth">Object to check</param>
        /// <param name="jwtkey">Token of the User for Auth</param>
        /// <returns>PasswordFound(Bool) and Priority(int)</returns>
        [HttpPut]
        public IActionResult ApplicantIsAuthorized(
            [FromQuery] string apikey,
            [FromBody] ApplicationAuth applicationAuth,
            [FromHeader] string jwtkey)
        {
            if (this.auth.KeyIsValid(apikey) && this.jwtService.PermissionLevelValid(jwtkey, "user"))
            {
                var authForConf = this._context.ApplicationAuth.Where(a => a.Conference_ID == applicationAuth.Conference_ID
                                                                        && a.Council_ID == applicationAuth.Council_ID
                                                                        && a.Password == applicationAuth.Password).FirstOrDefault();
                if (authForConf == null)
                {
                    return this.Ok(new { PasswordFound = false, Priority = 0 });
                }
                else
                {
                    return this.Ok(new { PasswordFound = true, Prioriy = authForConf.Priority });
                }
            }

            return this.Unauthorized();
        }

        private bool ApplicationAuthExists(int id)
        {
            return this._context.ApplicationAuth.Any(e => e.ID == id);
        }
    }
}