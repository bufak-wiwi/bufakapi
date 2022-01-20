namespace BuFaKAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using BuFaKAPI.Models;
    using BuFaKAPI.Models.SubModels;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using WebApplication1.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class PublicDataController : ControllerBase
    {
        private readonly MyContext _context;

        // private readonly string apikey;
        // private readonly TelegramBot telBot;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public PublicDataController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;

            // this.telBot = new TelegramBot();
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/export/complete/1

        /// <summary>
        /// Gets the number of all Applications for a specific conference
        /// </summary>
        /// <param name="conference_id">ID of the Conference</param>
        /// <param name="apikey">API Key for authentification</param>

        /// <returns>number of all Applications for a specific conference</returns>
        [HttpGet("numberofapplications/{conference_id}")]
        public IActionResult GetAll(
            [FromRoute] int conference_id,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if ( this.auth.KeyIsValid(apikey, conference_id))
            {
                return this.Ok(this._context.Conference_Application.Where(ca => ca.ConferenceID == conference_id).Count());
            }

            return this.Unauthorized();
        }
    }
}