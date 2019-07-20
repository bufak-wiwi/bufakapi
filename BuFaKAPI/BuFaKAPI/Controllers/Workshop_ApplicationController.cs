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
    public class Workshop_ApplicationController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TelegramBot telBot;
        private readonly TokenService jwtService;

        public Workshop_ApplicationController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.telBot = new TelegramBot();
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/Workshop_Application

        /// <summary>
        /// Gets all Workshop_Applications from one specific Conference
        /// </summary>
        /// <param name="conference_id">The Conference in Question</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>A List of Workshop_Application Objects</returns>
        [HttpGet("conference/{conference_id}")]
        public IActionResult GetWorkshop_Applications(
            [FromRoute] int conference_id,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, conference_id))
            {
                var workshops_conference = this._context.Workshop.Where(w => w.ConferenceID == conference_id);
                List<Workshop_Application> wa = new List<Workshop_Application>();
                foreach (Workshop ws in workshops_conference)
                {
                    foreach (Workshop_Application wap in this._context.Workshop_Application.Where(wapplic => wapplic.WorkshopID == ws.WorkshopID))
                    {
                        wa.Add(wap);
                    }
                }

                return this.Ok(wa);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Gets the Information, if a specific User has already applied for Workshops
        /// </summary>
        /// <param name="conference_id">ID of the conference to restrict the Search to</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="uid">ID of the User to get the Workshop_Applications from</param>
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
                List<Workshop_Application> wa = this._context.Workshop_Application.Where(w => w.ApplicantUID == uid).ToList();
                return this.Ok(wa);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Gets an Overview over how many people voted for the Workshops at what priority
        /// </summary>
        /// <param name="conference_id">ID of the conference to restrict the Search to</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>A List of Applications for each Workshop</returns>
        [HttpGet("overview/")]
        public IActionResult GetWorkshop_Application_Overview(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey, conference_id))
            {
                List<Workshop_Application> ws_applications = this._context.Workshop_Application.ToList();
                List<Workshop> workshops = this._context.Workshop.ToList();

                var query = ws_applications.Join(
                    workshops,
                    application => application.WorkshopID,
                    workshop => workshop.WorkshopID,
                    (workshop, application) => new
                    {
                        UID = workshop.ApplicantUID,
                        Workshop_ID = workshop.WorkshopID,
                        Conference_ID = application.ConferenceID
                    });
                int user_ws_applicant = 0;

                List<WSApplication> applications = new List<WSApplication>();
                foreach (Workshop ws in this._context.Workshop.Where(ws => ws.ConferenceID == conference_id))
                {
                    var current_app_count = this._context.Workshop_Application.Where(workshop => workshop.WorkshopID == ws.WorkshopID).Count();
                    applications.Add(new WSApplication
                    {
                        Workshop_ID = ws.WorkshopID,
                        Workshop_Name = ws.Name,
                        Workshop_Hostname = ws.HostName,
                        Applications = current_app_count,
                        Applications_Prio_1 = this._context.Workshop_Application.Where(workshop => workshop.WorkshopID == ws.WorkshopID && workshop.Priority == 1).Count(),
                        Applications_Prio_2 = this._context.Workshop_Application.Where(workshop => workshop.WorkshopID == ws.WorkshopID && workshop.Priority == 2).Count(),
                        Applications_Prio_3 = this._context.Workshop_Application.Where(workshop => workshop.WorkshopID == ws.WorkshopID && workshop.Priority == 3).Count(),
                    });
                }

                var overview = new
                {
                    User_WS_Applications = user_ws_applicant,
                    Application_Count = query.Where(q => q.Conference_ID == conference_id).Count(),
                    Applications = applications
            };
                return this.Ok(overview);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Updates a specific Workshop_Application object
        /// </summary>
        /// <param name="workshop_Application">Workshop_Application Object to be updated</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>No Content</returns>
        /// <response code="400">If the ModelState is not valid</response>
        /// <response code="401">If the API Key is not valid</response>
        /// <response code="404">If the to-be-updated Workshop_Application Object does not exist</response>
        [HttpPut]
        public async Task<IActionResult> PutWorkshop_Application(
            [FromBody] Workshop_Application workshop_Application,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            // TODO add History
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this._context.Entry(workshop_Application).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.Workshop_ApplicationExists(workshop_Application.WorkshopID, workshop_Application.ApplicantUID))
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
        /// Adds another Workshop_Application to the Database
        /// </summary>
        /// <param name="applications">A List of Workshop-Applications</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>The newly added workshop_application Object</returns>
        /// <response code="400">If the ModelState is not valid</response>
        /// <response code="401">If the API Key is not valid</response>
        /// <response code="404">If the Workshop or the User is not in the Database</response>
        [HttpPost]
        public async Task<IActionResult> PostWorkshop_Application(
            [FromBody] List<BulkApplication> applications,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                var uid = string.Empty;
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                foreach (BulkApplication application in applications)
                {
                    User user = this._context.User.Where(u => u.UID == application.Applicant_UID).FirstOrDefault();
                    uid = user.UID;
                    Workshop workshop = this._context.Workshop.Where(ws => ws.WorkshopID == application.Workshop_ID).FirstOrDefault();
                    if (workshop == null || user == null)
                    {
                        return this.NotFound();
                    }

                    Workshop_Application wa = new Workshop_Application
                    {
                        User = user,
                        ApplicantUID = application.Applicant_UID,
                        WorkshopID = application.Workshop_ID,
                        Workshop = workshop,
                        Priority = application.Priority,
                    };
                    this._context.Workshop_Application.Add(wa);
                }

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    /*if (this.Workshop_ApplicationExists(workshop_Application.WorkshopID, workshop_Application.ApplicantUID))
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                    else
                    {
                        throw;
                    }*/
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                this.telBot.SendTextMessage($"New Workshop Application from {uid}");
                return this.Ok();

                // return this.CreatedAtAction("GetWorkshop_Application", new { id = workshop_Application.WorkshopID }, workshop_Application);
            }

            return this.Unauthorized();
        }

        private bool Workshop_ApplicationExists(int workshop_id, string uid)
        {
            return this._context.Workshop_Application.Any(e => e.WorkshopID == workshop_id && e.ApplicantUID == uid);
        }
    }
}