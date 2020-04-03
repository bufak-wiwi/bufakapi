using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuFaKAPI.Models;
using BuFaKAPI.Services;
using Microsoft.Extensions.Options;

namespace BuFaKAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingAnswersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TelegramBot telBot;
        private readonly TokenService jwtService;

        public VotingAnswersController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(this._context);
            this.telBot = new TelegramBot();
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/VotingAnswers/5
        [HttpGet("byQuestion/{id}")]
        public async Task<IActionResult> GetVotingAnswer(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromRoute] int id,
            [FromQuery] string apikey)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                return this.Ok(this._context.VotingAnswer.Where(x => x.QuestionID == id).ToList());
            }
                return this.Unauthorized();
        }

        // POST: api/VotingAnswers
        [HttpPost]
        public async Task<IActionResult> PostVotingAnswer(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromBody] VotingAnswer votingAnswer)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return BadRequest(this.ModelState);
                }

                this._context.VotingAnswer.Add(votingAnswer);
                await this._context.SaveChangesAsync();

                return this.CreatedAtAction("GetVotingAnswer", new { id = votingAnswer.AnswerID }, votingAnswer);
            }

            return this.Unauthorized();
        }

        private bool VotingAnswerExists(int id)
        {
            return _context.VotingAnswer.Any(e => e.AnswerID == id);
        }
    }
}