namespace BuFaKAPI.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using BuFaKAPI.Models.SubModels;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using WebApplication1.Models;

    public class ConferencePhases
    {
        public bool ConferenceApplicationPhase { get; set; }

        public bool WorkshopApplicatonPhase { get; set; }

        public bool WorkshopSuggestionPhase { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ConferencesController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;
        private readonly TelegramBot telBot;

        public ConferencesController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(this._context, settings);
            this.telBot = new TelegramBot();
        }

        /// <summary>
        /// Gets all the Conferences from the API
        /// </summary>
        /// <param name="apikey">API Key for the Conference</param>
        /// <param name="jwttoken">Token of the User for Auth</param>
        /// <returns>myreturn</returns>
        // GET: api/Conferences
        [HttpGet]
        public IActionResult GetConference(
            [FromQuery] string apikey,
            [FromHeader] string jwttoken)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                return this.Ok(this._context.Conference);
            }

            return this.Unauthorized();
        }

        // GET: api/Conferences/5

        /// <summary>
        /// Gets a single Conference-Object
        /// </summary>
        /// <param name="id">The ID of the Conference</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken">JWT Token of the User for Auth</param>
        /// <returns>A Single Conference-Object</returns>
        /// <response code="400">If ModelState is not valid</response>
        /// <response code="401">If API Key is not valid</response>
        /// <response code="404">If ID is not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConference([FromRoute] int id, [FromQuery] string apikey, [FromHeader] string jwttoken)
        {
            // First Check if the API Key is valid and if the user has the permission level "user"
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var conference = await this._context.Conference.FindAsync(id);

                if (conference == null)
                {
                    return this.NotFound();
                }

                return this.Ok(conference);
            }

            return this.Unauthorized();
        }

        // PUT: api/Conferences/5

        /// <summary>
        /// Update a specific Conference
        /// </summary>
        /// <param name="id">ID of the Conference to be Updated</param>
        /// <param name="conference">New Conference-Object</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken">JWT Token of the user for Auth</param>
        /// <returns>No Content</returns>
        /// <response code="400">If ModelState is not valid</response>
        /// <response code="401">If API Key is not valid</response>
        /// <response code="404">If ID of the Conference is not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConference(
            [FromRoute] int id,
            [FromBody] Conference conference,
            [FromQuery] string apikey,
            [FromHeader] string jwttoken)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != conference.ConferenceID)
                {
                    return this.BadRequest();
                }

                this._context.Entry(conference).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.ConferenceExists(id))
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

            return this.Unauthorized();
        }

        /// <summary>
        /// Creates a new Conference, if the user who demands it is allowed to do it
        /// </summary>
        /// <param name="apikey">apikey for Authentification</param>
        /// <param name="pconf">PConf Object, containing the </param>
        /// <param name="jwttoken">jwttoken of the responsible logged-in user</param>
        /// <response code="401">If API Key is not valid or jwtToken is not valid or user is not superuser</response>
        /// <returns>the created conference</returns>
        [HttpPost]
        public async Task<IActionResult> PostConference(
                                                        [FromQuery] string apikey,
                                                        [FromBody] PostConference pconf,
                                                        [FromHeader] string jwttoken)
        {
            if (
                this.auth.KeyIsValid(apikey)
                && this.jwtService.PermissionLevelValid(jwttoken, "superadmin")
                )
            {
                this._context.Conference.Add(pconf.conference);
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        throw;
                }

                var newapikey = this.jwtService.GenerateApiKey();
                var validUntil = DateTime.Parse(pconf.conference.DateEnd).AddDays(60).ToString();

                // create a new api key with metadata and write it to the database
                Auth auth = new Auth
                {
                    ApiKey = newapikey,
                    ValidUntil = validUntil,
                    CreatedOn = DateTime.Now.ToString(),
                    Note = pconf.note,
                    ConferenceID = pconf.conference.ConferenceID
                };
                this._context.Auth.Add(auth);
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return this.CreatedAtAction("GetConference", new { id = pconf.conference.ConferenceID }, pconf.conference);
            }

            return this.Unauthorized();
        }

        [HttpPut("phases/")]

        public async Task<IActionResult> UpdatePhasesForConference(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromBody] ConferencePhases conferencephases)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                Conference conf = this._context.Conference.FindAsync(conference_id).Result;
                conf.ConferenceApplicationPhase = conferencephases.ConferenceApplicationPhase;
                conf.WorkshopApplicationPhase = conferencephases.WorkshopApplicatonPhase;
                conf.WorkshopSuggestionPhase = conferencephases.WorkshopSuggestionPhase;

                this._context.Entry(conf).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.ConferenceExists(conference_id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.Ok(conf);
            }
            return this.Unauthorized();
        }

        private bool ConferenceExists(int id)
        {
            return this._context.Conference.Any(e => e.ConferenceID == id);
        }
    }
}