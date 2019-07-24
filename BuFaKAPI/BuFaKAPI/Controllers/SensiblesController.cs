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

    [Route("api/[controller]")]
    [ApiController]
    public class SensiblesController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TelegramBot telBot;
        private readonly TokenService jwtService;

        public SensiblesController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(this._context);
            this.telBot = new TelegramBot();
            this.jwtService = new TokenService(this._context, settings);
        }

        /// <summary>
        /// Gets the valid Sensible Object vor a specific user
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="uid">ID of the specific User</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>401 if API Key is not correct</returns>
        [HttpGet("forUser/{uid}")]
        public IActionResult GetSensibleForUser(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromRoute] string uid,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                Sensible currentSensible = this._context.Sensible.Where(s => s.ConferenceID == conference_id
                                                                     && s.UID == uid
                                                                     && s.Invalid == false).LastOrDefaultAsync().Result;
                return this.Ok(currentSensible);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Gets all valid Sensible Objects for a specific Conference.
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for authentification</param>
        /// <returns>401 if API Key not valid</returns>
        [HttpGet("forConference/")]
        public IActionResult GetSensibleForConference(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                List<Sensible> sensibles = this._context.Sensible.Where(s => s.ConferenceID == conference_id
                                                                && s.Invalid == false).ToList();
                return this.Ok(sensibles);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Changes a Sensible Object and creates a dataset in the History Table
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="sensible">Sensible Object to be changed</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <returns>Nothing</returns>
        /// <response code="401">If API Key or JWTToken are not valid</response>
        [HttpPut]
        public async Task<IActionResult> PutSensible(
                                                        [FromHeader(Name = "conference_id")] int conference_id,
                                                        [FromBody] Sensible sensible,
                                                        [FromQuery] string apikey,
                                                        [FromHeader(Name = "jwttoken")] string jwttoken)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                Sensible oldSensible = this._context.Sensible.FindAsync(sensible.SensibleID).Result;
                string responsibleUID = this.jwtService.GetUIDfromJwtKey(jwttoken);
                History history = new History
                {
                    ResponsibleUID = responsibleUID,
                    User = this._context.User.FindAsync(responsibleUID).Result,
                    OldValue = oldSensible.ToString(),
                    HistoryType = "Edit"
                };
                oldSensible = sensible;
                await this._context.History.AddAsync(history);
                this._context.Entry(oldSensible).State = EntityState.Modified;
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return this.Ok();
            }

            return this.Unauthorized();
        }

        private bool SensibleExists(int id)
        {
            return this._context.Sensible.Any(e => e.SensibleID == id);
        }
    }
}