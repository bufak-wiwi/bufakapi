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
    using WebApplication1.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public WorkshopsController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/Workshops

        /// <summary>
        /// Gets all Workshops for a specific conference
        /// </summary>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="conference_id">ID of the Conference to get Workshops from</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <returns>List of Workshop-Objects on Success</returns>
        /// <response code="401">Unauthorized if API Key not valid</response>
        [HttpGet]
        public IActionResult GetWorkshop(
            [FromQuery] string apikey,
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey, conference_id))
            {
                return this.Ok(this._context.Workshop.Where(ws => ws.ConferenceID == conference_id && !ws.Invalid));
            }

            return this.Unauthorized();
        }

        // GET: api/Workshops/5

        /// <summary>
        /// Gets one specific Workshop-Object from the API
        /// </summary>
        /// <param name="id">ID of the Workshop to be Get</param>
        /// <param name="jwttoken"></param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>Workshop-Object on Success</returns>
        /// <response code="400">Bad Request if ModelState is not valid</response>
        /// <response code="401">Unathorized if API Key is not valid</response>
        /// <response code="404">Not Found if Workshop-ID is not in the Database</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkshop(
            [FromRoute] int id,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level User
            var workshop = await this._context.Workshop.FindAsync(id);
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey, workshop.ConferenceID))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (workshop == null)
                {
                    return this.NotFound();
                }

                return this.Ok(workshop);
            }

            return this.Unauthorized();
        }

        // PUT: api/Workshops/5

        /// <summary>
        /// Update one specific Workshop in the Database
        /// </summary>
        /// <param name="id">ID of the Workshop to be updated</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="workshop">Workshop-Object to be Updated</param>
        /// <returns>Updated Workshop-Object on Success</returns>
        /// <response code="400">Bad Request if Model State is not valid</response>
        /// <response code="401">Unauthorized if API Key is not valid</response>
        /// <response code="404">Not found if Workshop-ID is not in the Database</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkshop(
            [FromRoute] int id,
            [FromQuery] string apikey,
            [FromHeader] string jwttoken,
            [FromBody] Workshop workshop)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, workshop.ConferenceID))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != workshop.WorkshopID)
                {
                    return this.BadRequest();
                }

                if (!string.IsNullOrWhiteSpace(workshop.HostUID))
                {
                    workshop.User = this._context.User.Where(u => u.UID == workshop.HostUID).FirstOrDefault();
                    workshop.HostName = workshop.User.Surname + " " + workshop.User.Name;
                }

                this._context.Entry(workshop).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.WorkshopExists(id))
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

        // POST: api/Workshops

        /// <summary>
        /// Adds a Workshop to the Database
        /// </summary>
        /// <param name="workshop">Workshop Element to be added</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">APIKey for the access</param>
        /// <returns>Created Item on Success</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="401">when APIKey is not Correct</response>
        /// <response code="400">when Workshop Model is not Valid</response>
        [HttpPost]
        public async Task<IActionResult> PostWorkshop(
            [FromBody] Workshop workshop,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, workshop.ConferenceID))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (!string.IsNullOrWhiteSpace(workshop.HostUID))
                {
                    workshop.User = this._context.User.Where(u => u.UID == workshop.HostUID).FirstOrDefault();
                    workshop.HostName = workshop.User.Surname + " " + workshop.User.Name;
                }

                workshop.Conference = this._context.Conference.Where(c => c.ConferenceID == workshop.ConferenceID).FirstOrDefault();
                this._context.Workshop.Add(workshop);
                await this._context.SaveChangesAsync();

                return this.CreatedAtAction("GetWorkshop", new { id = workshop.WorkshopID }, workshop);
            }

            return this.Unauthorized();
        }

        // DELETE: api/Workshops/5

        /// <summary>
        /// Deletes a specific Workshop
        /// </summary>
        /// <param Name="id">Id of the Workshop to be deleted</param>
        /// <param Name="apikey">API Key for Authentification</param>
        /// <returns>200 OK for Success</returns>
        /// <response code="400">Bad Request if model is not valid</response>
        /// <response code="401">Unauthorized if API Key not valid</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkshop([FromRoute] int id, 
                                                        [FromQuery] string apikey, 
                                                        [FromHeader] string jwttoken)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var workshop = await this._context.Workshop.FindAsync(id);
                var uid = this.jwtService.GetUIDfromJwtKey(jwttoken);
                if (!this._context.Administrator.Any(a => a.UID == uid && a.ConferenceID == workshop.ConferenceID))
                {
                    return this.Unauthorized();
                }

                if (workshop == null || workshop.Invalid)
                {
                    return this.NotFound();
                }

                workshop.Invalid = true;
                this._context.Entry(workshop).State = EntityState.Modified;
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.WorkshopExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.Ok(workshop);
            }

            return this.Unauthorized();
        }

        private bool WorkshopExists(int id)
        {
            return this._context.Workshop.Any(e => e.WorkshopID == id);
        }
    }
}