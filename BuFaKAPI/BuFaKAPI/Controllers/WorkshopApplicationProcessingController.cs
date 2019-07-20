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

    public class WorkshopOverview
    {
        public string Applicant_uid { get; set; }

        public int Workshop_id { get; set; }

        public string Host_uid { get; set; }

        public string Host_name { get; set; }

        public string Workshop_place { get; set; }

        public bool Is_rejected { get; set; }

        public bool Is_attendee { get; set; }

        public bool Is_helper { get; set; }

        public int Priority { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopApplicationProcessingController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public WorkshopApplicationProcessingController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(this._context, settings);
        }

        [HttpGet("{conference_id}")]
        public IActionResult GetOverview(
            [FromRoute] int conference_id,
            [FromHeader] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, conference_id))
            {
                List<WorkshopOverview> overview = new List<WorkshopOverview>();
                var workshops = this._context.Workshop.Where(w => w.ConferenceID == conference_id);
                foreach (Workshop ws in workshops)
                {
                    var applications = this._context.Workshop_Application.Where(wa => wa.WorkshopID == ws.WorkshopID);
                    foreach (Workshop_Application wa in applications)
                    {
                        WorkshopOverview ov = new WorkshopOverview
                        {
                            Applicant_uid = wa.ApplicantUID,
                            Workshop_id = ws.WorkshopID,
                            Workshop_place = ws.Place,
                            Host_uid = ws.HostUID,
                            Host_name = ws.HostName,
                            Priority = wa.Priority,
                            /*Is_rejected = wa.IsRejected,
                            Is_attendee = this._context.Workshop_Attendee.Any(wap => wap.attendee_uid == wa.ApplicantUID && wap.workshop_id == wa.WorkshopID),
                            IsHelper = this._context.Workshop_Attendee.Any(wat => wat.attendee_uid == wa.ApplicantUID && wat.workshop_id == wa.WorkshopID) ? this._context.Workshop_Attendee.Where(wat => wat.attendee_uid == wa.ApplicantUID && wat.workshop_id == wa.WorkshopID).FirstOrDefault().is_helper : false
                    */};

                        overview.Add(ov);
                    }
                }

                return this.Ok(overview);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /*[HttpPut("{conference_id}")]
        public async Task<IActionResult> PutWorkshopApplicationProcessing([FromRoute] int conference_id, [FromQuery] string apikey, [FromBody] List<WorkshopOverview> overview)
        {
            // Permission Level Admin
            if (this.auth.KeyIsValid(apikey, conference_id))
            {
                foreach (WorkshopOverview ov in overview)
                {
                    if (ov.Is_attendee)
                    {
                        // wenn nicht in datenbank, packs rein
                        if (!this._context.Workshop_Attendee.Any(wat => wat.attendee_uid == ov.ApplicantUID && wat.workshop_id == ov.Workshop_id))
                        {
                            Workshop_Attendee current_WA = new Workshop_Attendee
                            {
                                workshop_id = ov.Workshop_id,
                                attendee_uid = ov.ApplicantUID,
                                User = this._context.User.Where(u => u.UID == ov.ApplicantUID).FirstOrDefault(),
                                is_helper = ov.IsHelper
                            };
                            this._context.Add(current_WA);
                        }
                    }
                    else
                    {
                        // wenn in Datenbank, nimms raus
                        if (this._context.Workshop_Attendee.Any(wat => wat.attendee_uid == ov.ApplicantUID && wat.workshop_id == ov.Workshop_id))
                        {
                            Workshop_Attendee current_WA = this._context.Workshop_Attendee.Where(wat => wat.attendee_uid == ov.ApplicantUID && wat.workshop_id == ov.Workshop_id).FirstOrDefault();
                            this._context.Remove(current_WA);
                        }
                    }

                    Workshop_Application current_WAP = this._context.Workshop_Application.Where(wap => wap.WorkshopID == ov.Workshop_id && wap.ApplicantUID == ov.Applicant_uid).FirstOrDefault();
                    // current_WAP.IsRejected = ov.Is_rejected;
                }

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                      Do nothing
                }

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }*/
    }
}