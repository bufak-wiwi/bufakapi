namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using BuFaKAPI.Models.SubModels;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using WebApplication1.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public TravelController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(this._context, settings);
        }


        /// <summary>
        /// Adds travel infos to the database
        /// </summary>
        /// <param name="travel">Travel Element to be added</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">APIKey for the access</param>
        /// <returns>Created Item on Success</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="401">when APIKey is not Correct</response>
        /// <response code="400">when Workshop Model is not Valid</response>
        [Route("Suggestion")]
        [HttpPost]
        public async Task<IActionResult> PostWorkshopSuggestion(
            [FromBody] Travel travel,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user"))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                string uid = this.jwtService.GetUIDfromJwtKey(jwttoken);
               // User user = this._context.User.Find(uid);

                // Check if user is the host of the suggested workshop and the time is set to the default suggestion time
                //if (
                //    workshop.HostUID != user.UID ||
                //    workshop.HostName != $"{user.Name} {user.Surname}" ||
                //    workshop.Start != "2020-12-31T23:59")
                //{
                //    return this.BadRequest(this.ModelState);
                //}

               // travel.Conference = this._context.Conference.Where(c => c.ConferenceID == travel.ConferenceID).FirstOrDefault();
                this._context.Travel.Add(travel);
                await this._context.SaveChangesAsync();

                return this.CreatedAtAction("TravelInformations", new { id = travel.TravelID }, travel);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Gets the travel informations for a specific user
        /// </summary>
        /// <param name="conference_id">ID of the conference to restrict the Search to</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="uid">ID of the User to get the Travel informations from</param>
        /// <returns>An Object with Flags, indicating the Conference application and the Workshop application</returns>
        [HttpGet("peruser/{uid}")]
        public IActionResult GetWorkshop_Application_For_User(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromRoute] string uid)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey, conference_id))
            {
                var travel = this._context.Travel.FromSql($"SELECT * from Travel where UID = {uid} AND ConferenceID = {conference_id}").FirstOrDefault();

                if (travel == null)
                {
                    return this.NotFound();
                }

                return this.Ok(travel);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Gets the travel informations for a conference
        /// </summary>
        /// <param name="conference_id">ID of the conference to restrict the Search to</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>An Object with Flags, indicating the Conference application and the Workshop application</returns>
        [HttpGet("forconference")]
        public IActionResult GetTravel_Infos_For_Conference(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, conference_id))
            {
                var travel = this._context.Travel.FromSql($"SELECT Travel.TravelID,User.Name,User.Surname,Travel.Transportation,Travel.ParkingSpace,Travel.ArrivalTimestamp,Travel.ArrivalPlace,Travel.DepartureTimestamp,Travel.ExtraNote,Travel.ConferenceID,Travel.UID from Travel INNER JOIN User ON Travel.UID = User.UID where Travel.ConferenceID = {conference_id}").ToList();
                Console.WriteLine("-------------------");
                Console.WriteLine(travel);
                Console.WriteLine("-----------------------");
                if (travel == null)
                {
                    return this.NotFound();
                }

                return this.Ok(travel);
            }

            return this.Unauthorized();
        }



        private bool WorkshopExists(int id)
        {
            return this._context.Workshop.Any(e => e.WorkshopID == id);
        }
    }
}
