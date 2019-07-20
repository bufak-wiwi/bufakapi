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

    [Route("api/[controller]")]
    [ApiController]
    public class ConferencesController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public ConferencesController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(settings);
        }

        /// <summary>
        /// Gets all the Conferences from the API
        /// </summary>
        /// <param name="apikey">API Key for the Conference</param>
        /// <returns>myreturn</returns>
        // GET: api/Conferences
        [HttpGet]
        public IActionResult GetConference([FromQuery] string apikey)
        {
            //TODO Permission Level User
            if (this.auth.KeyIsValid(apikey))
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
        /// <returns>A Single Conference-Object</returns>
        /// <response code="400">If ModelState is not valid</response>
        /// <response code="401">If API Key is not valid</response>
        /// <response code="404">If ID is not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConference([FromRoute] int id, [FromQuery] string apikey)
        {
            //TODO Permission Level User
            if (this.auth.KeyIsValid(apikey))
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
        /// <returns>No Content</returns>
        /// <response code="400">If ModelState is not valid</response>
        /// <response code="401">If API Key is not valid</response>
        /// <response code="404">If ID of the Conference is not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConference([FromRoute] int id, [FromBody] Conference conference, [FromQuery] string apikey)
        {
            // TODO jwtToken einfügen - Level SuperAdmin
            if (this.auth.KeyIsValid(apikey))
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
        /// <param name="token">jwttoken of the responsible logged-in user</param>
        /// <response code="401">If API Key is not valid or jwtToken is not valid or user is not superuser</response>
        /// <returns>the created conference</returns>
        [HttpPost]
        public async Task<IActionResult> PostConference(
                                                        [FromQuery] string apikey,
                                                        [FromBody] PostConference pconf,
                                                        [FromHeader] string token)
        {
            //TODO Permission Level SuperAdmin
            string uid = this.jwtService.GetUIDfromJwtKey(token);
            if (
                this.auth.KeyIsValid(apikey)
                && this.auth.IsSuperAdmin(uid)
                && this.jwtService.ValidateJwtKey(token)
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

        private bool ConferenceExists(int id)
        {
            return this._context.Conference.Any(e => e.ConferenceID == id);
        }
    }
}