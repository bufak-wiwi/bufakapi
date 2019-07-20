namespace BuFaKAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using BuFaKAPI.Models;
    using BuFaKAPI.Models.SubModels;
    using BuFaKAPI.Services;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly string apikey;
        private readonly TelegramBot telBot;
        private readonly AuthService auth;

        public ExportController(MyContext context)
        {
            this._context = context;
            this.telBot = new TelegramBot();
            this.auth = new AuthService(context);
        }

        // GET: api/export/complete/1

        /// <summary>
        /// Gets an Export of all the ApplicantData of one Conference
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="apikey">API Key for authentification</param>
        /// <returns>19 Attributes in one Object</returns>
        [HttpGet("complete/{conference_id}")]
        public IActionResult GetAll([FromRoute] int conference_id, [FromQuery] string apikey)
        {
            // TODO Permission Level Admin
            if (this.auth.KeyIsValid(apikey, conference_id))
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
        /// TODO: Gets an Export of all the ApplicantData of one Conference
        /// </summary>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="apikey">API Key for authentification</param>
        /// <returns>19 Attributes in one Object</returns>
        [HttpGet("badges")]
        public IActionResult GetBadges([FromHeader] int conference_id, [FromQuery] string apikey)
        {
            // TODO Permission Level Admin
            if (this.auth.KeyIsValid(apikey, conference_id))
            {
                return this.Ok();
            }

            return this.Unauthorized();
        }
    }
}