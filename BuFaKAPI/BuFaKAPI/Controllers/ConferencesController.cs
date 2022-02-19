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

        public bool WorkshopApplicationPhase { get; set; }

        public bool WorkshopSuggestionPhase { get; set; }

        public string InformationTextConferenceApplication { get; set; }

        public string InformationTextWorkshopSuggestion { get; set; }

        public string LinkParticipantAgreement { get; set; }

        public string WorkshopDurations { get; set; }

        public string WorkshopTopics { get; set; }

        public string TravelArrivalPlaces { get; set; }
        public string TravelTransportation { get; set; }
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
            if (string.IsNullOrEmpty(jwttoken) || string.IsNullOrEmpty(apikey))
            {
                return this.NoContent();
            }
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
                                                        [FromBody] Conference conference,
                                                        [FromHeader] string jwttoken)
        {
            if (
                this.auth.KeyIsValid(apikey)
                && this.jwtService.PermissionLevelValid(jwttoken, "superadmin"))
            {
                this._context.Conference.Add(conference);
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        throw;
                }

                return this.CreatedAtAction("GetConference", new { id = conference.ConferenceID }, conference);
            }

            return this.Unauthorized();
        }

        [HttpPut("phases/{conference_id}")]

        public async Task<IActionResult> UpdatePhasesForConference(
            [FromRoute] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromBody] ConferencePhases conferencephases)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                Conference conf = this._context.Conference.FindAsync(conference_id).Result;
                conf.ConferenceApplicationPhase = conferencephases.ConferenceApplicationPhase;
                conf.WorkshopApplicationPhase = conferencephases.WorkshopApplicationPhase;
                conf.WorkshopSuggestionPhase = conferencephases.WorkshopSuggestionPhase;
                conf.InformationTextConferenceApplication = conferencephases.InformationTextConferenceApplication;
                conf.InformationTextWorkshopSuggestion = conferencephases.InformationTextWorkshopSuggestion;
                conf.LinkParticipantAgreement = conferencephases.LinkParticipantAgreement;
                conf.WorkshopDurations = conferencephases.WorkshopDurations;
                conf.WorkshopTopics = conferencephases.WorkshopTopics;
                conf.TravelArrivalPlaces = conferencephases.TravelArrivalPlaces;
                conf.TravelTransportation = conferencephases.TravelTransportation;


                this._context.Entry(conf).State = EntityState.Modified;

                if (!(conferencephases.InformationTextWorkshopSuggestion is null) && (conferencephases.InformationTextWorkshopSuggestion.Contains("<script>") || conferencephases.InformationTextConferenceApplication.Contains("<script>")))
                {
                    return this.Unauthorized();
                }

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