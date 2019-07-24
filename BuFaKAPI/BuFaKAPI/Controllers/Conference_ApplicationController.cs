// <copyright file="Conference_ApplicationController.cs" company="BuFaKWiSo">
// Copyright (c) BuFaKWiSo. All rights reserved.
// </copyright>

namespace BuFaKAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
    public class Conference_ApplicationController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TelegramBot telBot;
        private readonly TokenService jwtService;

        public Conference_ApplicationController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(context);
            this.telBot = new TelegramBot();
            this.jwtService = new TokenService(this._context, settings);
        }

        /// <summary>
        /// Gets the Conference Applications from one specific Conference
        /// </summary>
        /// <param name="jwttoken">Token of the User for Auth</param>
        /// <param name="conference_id">ID of the Conference in Question</param>
        /// <param name="apikey">API Key for Authentification</param>
        /// <returns>A List of Conference_Applications</returns>
        /// <response code="401">If API Key is not valid</response>
        [HttpGet("forConference/")]
        public IActionResult GetConference_Application(
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromQuery] string apikey)
        {
            // Permission Level: Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey, conference_id))
            {
                var users = this._context.Conference_Application.Where(c => c.ConferenceID == conference_id);
                return this.Ok(users);
            }

            return this.Unauthorized();
        }

        [HttpGet("single/{uid}")]
        public IActionResult GetSingleConferenceApplication(
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromQuery] string apikey,
            [FromRoute] string uid)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                return this.Ok(this._context.Conference_Application.Where(ca => ca.ApplicantUID == uid && ca.ConferenceID == conference_id));
            }

            return this.Unauthorized();
        }

        // POST: api/Conference_Application

        /// <summary>
        /// Adds a new Application to the current Conference
        /// </summary>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="application">Application Object needed for adding the Application</param>
        /// <param name="jwttoken">User Token for Auth</param>
        /// <returns>201 on Success</returns>
        /// <response code="400">If ModelState is not valid</response>
        /// <response code="401">If API Key is not valid</response>
        /// <response code="409">If something went wrong at the Database connection</response>
        [HttpPost]
        public async Task<IActionResult> PostConference_Application(
                                                                    [FromQuery] string apikey,
                                                                    [FromBody] IncomingApplication application,
                                                                    [FromHeader] string jwttoken)
        {
            // Permission Level: User
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                Sensible sensible = new Sensible
                {
                    BuFaKCount = application.Count,
                    ConferenceID = application.ConferenceID,
                    Timestamp = DateTime.Now.ToString(),
                    UID = application.ApplicantUID,
                    EatingPreferences = application.Eating,
                    Intolerances = application.Intolerance,
                    SleepingPreferences = application.SleepingPref,
                    Telephone = application.Tel,
                    ExtraNote = application.Note
                };
                var sensibleID = this.InsertNewSensibleAsync(sensible).Result;

                Conference_Application cf = new Conference_Application
                {
                    ConferenceID = application.ConferenceID,
                    Conference = await this._context.Conference.FindAsync(application.ConferenceID),
                    ApplicantUID = application.ApplicantUID,
                    User = await this._context.User.FindAsync(application.ApplicantUID),
                    Priority = application.Priority,
                    IsAlumnus = application.IsAlumnus,
                    IsBuFaKCouncil = application.IsBuFaKCouncil,
                    IsHelper = application.IsHelper,
                    Note = application.Note,
                    Timestamp = DateTime.Now.ToString(),
                    Hotel = application.Hotel,
                    Room = application.Room,
                    Status = this.StatusToString(application.Status),
                    SensibleID = sensibleID,
                    Sensible = this._context.Sensible.FindAsync(sensibleID).Result
                };
                this._context.Conference_Application.Add(cf);
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (this.Conference_ApplicationExists(cf.ApplicantUID, cf.ConferenceID))
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.CreatedAtAction("GetConference_Application", new { id = cf.ConferenceID }, cf);
            }

            return this.Unauthorized();
        }

        /// <summary>
        /// Changes the current Status of a Conference Application
        /// Possible Status: 1: IsRejected, 2: IsAttendee
        /// </summary>
        /// <param name="apikey">API Key for Authentification</param>
        /// <param name="newstatus">Object for Identifying the Conference Application and the to-be-set Status</param>
        /// <param name="jwttoken">UID from the editing User</param>
        /// <returns>401 if api key not valid</returns>
        [HttpPut("status/")]
        public async Task<IActionResult> PutConference_ApplicationStatus(
            [FromQuery] string apikey,
            [FromBody] ChangeCAStatus newstatus,
            [FromHeader] string jwttoken)
        {
            // Permission Level admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                var thisca = this._context.Conference_Application.Where(ca => ca.ApplicantUID == newstatus.UID && ca.ConferenceID == newstatus.ConferenceID).FirstOrDefault();
                var responsibleUID = this.jwtService.GetUIDfromJwtKey(jwttoken);
                History history = new History
                {
                    ResponsibleUID = responsibleUID,
                    User = this._context.User.FindAsync(responsibleUID).Result,
                    OldValue = thisca.Status,
                    HistoryType = "Edit"
                };
                thisca.Status = this.StatusToString(newstatus.NewStatus);
                this._context.Entry(thisca).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.Conference_ApplicationExists(thisca.ApplicantUID, thisca.ConferenceID))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.Ok();
            }

            return this.Unauthorized();
        }

        [HttpPut("bulkstatus/")]
        public async Task<IActionResult> PutConference_ApplicationBulkStatus(
            [FromQuery] string apikey,
            [FromBody] ChangeCAStatusBulk newstati,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromHeader(Name = "conference_id")]int conference_id)
        {
            if (this.auth.KeyIsValid(apikey) && this.jwtService.PermissionLevelValid(jwttoken, "admin"))
            {
                foreach (string uid in newstati.UIDs)
                {
                    var thisca = this._context.Conference_Application.Where(ca => ca.ApplicantUID == uid && ca.ConferenceID == conference_id).FirstOrDefault();
                    var responsibleUID = this.jwtService.GetUIDfromJwtKey(jwttoken);
                    History history = new History
                    {
                        ResponsibleUID = responsibleUID,
                        User = this._context.User.FindAsync(responsibleUID).Result,
                        OldValue = thisca.Status,
                        HistoryType = "Edit"
                    };
                    thisca.Status = this.StatusToString(newstati.NewStatus);
                    this._context.Entry(thisca).State = EntityState.Modified;

                    try
                    {
                        await this._context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!this.Conference_ApplicationExists(thisca.ApplicantUID, thisca.ConferenceID))
                        {
                            this.telBot.SendTextMessage($"CA for {thisca.ApplicantUID} and {thisca.ConferenceID} not in Database");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return this.Ok();
            }

            return this.Unauthorized();


        }

        [HttpPut("reRegister")]
        public async Task<IActionResult> ReRegisterConference_Application(
                                                                          [FromQuery] string apikey,
                                                                          [FromBody] ReRegister reregister,
                                                                          [FromHeader(Name = "jwttoken")] string jwttoken,
                                                                          [FromHeader(Name = "conference_id")] int conferenceID)
        {
            // Permission Level Admin
            if (this.jwtService.PermissionLevelValid(jwttoken, "admin") && this.auth.KeyIsValid(apikey))
            {
                var currentCA = await this._context.Conference_Application.Where(ca => ca.ApplicantUID == reregister.OldUID
                                                                                    && ca.ConferenceID == conferenceID
                                                                                    && ca.Invalid == false).FirstOrDefaultAsync();
                string responsibleUID = this.jwtService.GetUIDfromJwtKey(jwttoken);
                History history = new History
                {
                    ResponsibleUID = responsibleUID,
                    User = this._context.User.FindAsync(responsibleUID).Result,
                    OldValue = currentCA.ToString(),
                    HistoryType = "Edit"
                };
                currentCA.Invalid = true;
                Conference_Application newCA = new Conference_Application
                {
                    ConferenceID = conferenceID,
                    Conference = this._context.Conference.FindAsync(conferenceID).Result,
                    ApplicantUID = reregister.NewUID,
                    User = this._context.User.FindAsync(reregister.NewUID).Result,
                };

                this._context.Conference_Application.Add(newCA);

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.Conference_ApplicationExists(newCA.ApplicantUID, newCA.ConferenceID))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.Ok();
            }

            return this.Unauthorized();
        }

        private bool Conference_ApplicationExists(string uid, int conference_id)
        {
            return this._context.Conference_Application.Any(e => e.ConferenceID == conference_id && e.ApplicantUID == uid);
        }

        private string StatusToString(CAStatus castatus)
        {
            if (castatus == CAStatus.HasApplied)
            {
                this.telBot.SendTextMessage("CAStatus equals 0");
                return "HasApplied";
            }
            else if (castatus == CAStatus.IsRejected)
            {
                this.telBot.SendTextMessage("CAStatus equals 1");
                return "IsRejected";
            }
            else if (castatus == CAStatus.IsAttendee)
            {
                this.telBot.SendTextMessage("CAStatus equals 2");
                return "IsAttendee";
            }
            else
            {
                this.telBot.SendTextMessage("CAStatus not valid");
                return null;
            }
        }

        private CAStatus StatusToObject(string castatus)
        {
            if (castatus.Equals("IsRejected"))
            {
                return CAStatus.IsRejected;
            }
            else if (castatus.Equals("IsAttendee"))
            {
                return CAStatus.IsAttendee;
            }
            else if (castatus.Equals("HasApplied"))
            {
                return CAStatus.HasApplied;
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        private async Task<int> InsertNewSensibleAsync(Sensible sensible)
        {
            List<Sensible> sensibles = this._context.Sensible.Where(se => se.ConferenceID == sensible.ConferenceID && se.UID == sensible.UID).ToList();
            if (sensibles.Count() != 0)
            {
                foreach (Sensible se in sensibles)
                {
                    se.Invalid = true;
                    this._context.Entry(se).State = EntityState.Modified;
                }

                await this._context.SaveChangesAsync();
            }

            await this._context.Sensible.AddAsync(sensible);
            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (this._context.Sensible.Any(s => s.ConferenceID == sensible.ConferenceID && s.UID == sensible.UID))
                {
                    throw new ArgumentException();
                }
                else
                {
                    throw new InvalidDataException();
                }
            }

            var currentSensible = this._context.Sensible.Where(s => s.ConferenceID == sensible.ConferenceID && s.UID == sensible.UID).LastOrDefaultAsync().Result;

            return currentSensible.SensibleID;
        }
    }
}