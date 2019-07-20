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
    public class ExportController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly string apikey;
        private readonly TelegramBot telBot;
        private readonly AuthService auth;
        private readonly TokenService jwtService;

        public ExportController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.telBot = new TelegramBot();
            this.auth = new AuthService(context);
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/export/complete/1

        /// <summary>
        /// Gets an Export of all the ApplicantData of one Conference
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="apikey">API Key for authentification</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <returns>19 Attributes in one Object</returns>
        [HttpGet("complete/{conference_id}")]
        public IActionResult GetAll(
            [FromRoute] int conference_id,
            [FromQuery] string apikey,
            [FromHeader] string jwttoken)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, conference_id))
            {
                List<CompleteExport> re = new List<CompleteExport>();
                var applications = this._context.Conference_Application.Where(ca => ca.ConferenceID == conference_id);
                foreach (Conference_Application ca in applications)
                {
                    var currentUser = this._context.User.Where(u => u.UID == ca.ApplicantUID).FirstOrDefault();
                    var currentCouncil = this._context.Council.Where(c => c.CouncilID == currentUser.CouncilID).FirstOrDefault();
                    CompleteExport ce = new CompleteExport()
                    {
                        UID = currentUser.UID,
                        Surname = currentUser.Surname,
                        Name = currentUser.Name,
                        Email = currentUser.Email,
                        Birthday = currentUser.Birthday,
                        Address = currentUser.Address,
                        Sex = currentUser.Sex,
                        Council = currentCouncil.Name,
                        CouncilCity = currentCouncil.City,
                        CouncilEmail = currentCouncil.ContactEmail,
                        CouncilState = currentCouncil.State,
                        University = currentCouncil.University,
                        UniversityAddress = currentCouncil.Address,
                        Note = ca.Note,
                        Priority = ca.Priority,
                        IsAlumnus = ca.IsAlumnus,
                        IsBuFaKCouncil = ca.IsBuFaKCouncil,
                        IsHelper = ca.IsHelper
                    };
                    re.Add(ce);
                }

                return this.Ok(re);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <param name="apikey">API Key for authentification</param>
        /// <returns>19 Attributes in one Object</returns>
        [HttpGet("badges")]
        public IActionResult GetBadges(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey, conference_id))
            {
                List<BadgeExport> badgeExport = new List<BadgeExport>();

                List<Conference_Application> attendees = this._context.Conference_Application.Where(ca => ca.ConferenceID == conference_id
                                                                                                        && ca.Status == "IsAttendee"
                                                                                                        && ca.Invalid == false).ToList();
                foreach (Conference_Application attendee in attendees)
                {
                    User currUser = this._context.User.FindAsync(attendee.ApplicantUID).Result;
                    Council currCouncil = this._context.Council.FindAsync(currUser.CouncilID).Result;
                    List<Workshop_Application> currWorkshops = this._context.Workshop_Application.Where(wa => wa.ApplicantUID == currUser.UID && wa.Status == "IsAttendee").ToList();
                    List<WorkshopExport> workshops = new List<WorkshopExport>();
                    foreach (Workshop_Application currWorkshop in currWorkshops)
                    {
                        Workshop ws = this._context.Workshop.Find(currWorkshop.WorkshopID);
                        WorkshopExport we = new WorkshopExport
                        {
                            Workshop_ID = currWorkshop.WorkshopID,
                            Name = ws.NameShort,
                            Start = ws.Start,
                            Place = ws.Place
                        };
                        workshops.Add(we);
                    }

                    BadgeExport be = new BadgeExport
                    {
                        UID = currUser.UID,
                        Name = currUser.Name,
                        Surname = currUser.Surname,
                        CouncilID = currCouncil.CouncilID,
                        CouncilName = currCouncil.NameShort,
                        University = currCouncil.UniversityShort,
                        IsHelper = attendee.IsHelper,
                        IsBuFaKCouncil = attendee.IsBuFaKCouncil,
                        IsAlumnus = attendee.IsAlumnus,
                        Workshops = workshops != null ? workshops : null
                    };
                    badgeExport.Add(be);
                }

                return this.Ok(badgeExport);
            }

            return this.Unauthorized();
        }
    }
}