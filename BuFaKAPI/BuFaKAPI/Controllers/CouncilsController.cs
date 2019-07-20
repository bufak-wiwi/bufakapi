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
    public class CouncilsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public CouncilsController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(this._context);
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/Councils

        /// <summary>
        /// Gets a list of all Councils in the Database
        /// </summary>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken">User Token for Authentification</param>
        /// <returns>A List of Council-Objects</returns>
        /// <response code="401">Unauthorized, if API Key is not valid</response>
        [HttpGet]
        public IActionResult GetCouncil(
            [FromQuery] string apikey,
            [FromHeader]string jwttoken)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                return this.Ok(this._context.Council);
            }

            return this.Unauthorized();
        }

        // GET: api/Councils/5

        /// <summary>
        /// Gets a specific council from the Database
        /// </summary>
        /// <param name="id">ID of the Council to be get</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>One Council-Object</returns>
        /// <response code="401">Unauthorized, if API Key is not valid</response>
        /// <response code="404">If ID of Council is not in the Database</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCouncil(
            [FromRoute] int id,
            [FromQuery] string apikey,
            [FromHeader] string jwttoken)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var council = await this._context.Council.FindAsync(id);

                if (council == null)
                {
                    return this.NotFound();
                }

                return this.Ok(council);
            }

            return this.Unauthorized();
        }

        // PUT: api/Councils/5

        /// <summary>
        /// Updates one Council in the Database
        /// </summary>
        /// <param name="id">ID of the Council to update</param>
        /// <param name="council">Council-Object for updating</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken"></param>
        /// <returns>No Content returned</returns>
        /// <response code="401">If API Key is not valid</response>
        /// <response code="404">If the to-be-updated Council is not found in the Database</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCouncil(
            [FromRoute] int id,
            [FromBody] Council council,
            [FromQuery] string apikey,
            [FromHeader] string jwttoken)
        {
            // Permission Level SuperAdmin
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != council.CouncilID)
                {
                    return this.BadRequest();
                }

                this._context.Entry(council).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.CouncilExists(id))
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

        // POST: api/Councils

        /// <summary>
        /// Adds one Council to the Database
        /// </summary>
        /// <param name="council">Council-Object to be added</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <returns>The created Council-Object</returns>
        /// <response code="400">If ModelState is not valid</response>
        /// <response code="401">If API Key is not valid</response>
        [HttpPost]
        public async Task<IActionResult> PostCouncil(
            [FromBody] Council council,
            [FromQuery] string apikey,
            [FromHeader] string jwttoken)
        {
            // Permission Level SuperAdmin
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this._context.Council.Add(council);
                await this._context.SaveChangesAsync();

                return this.CreatedAtAction("GetCouncil", new { id = council.CouncilID }, council);
            }

            return this.Unauthorized();
        }

        private bool CouncilExists(int id)
        {
            return this._context.Council.Any(e => e.CouncilID == id);
        }
    }
}