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
using BuFaKAPI.Models.SubModels;
using WebApplication1.Models;

namespace BuFaKAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingQuestionsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly AuthService auth;
        private readonly TelegramBot telBot;
        private readonly TokenService jwtService;

        public VotingQuestionsController(MyContext context, IOptions<AppSettings> settings)
        {
            this._context = context;
            this.auth = new AuthService(this._context);
            this.telBot = new TelegramBot();
            this.jwtService = new TokenService(this._context, settings);
        }

        // GET: api/VotingQuestions/5
        [HttpGet("byConference/{conference_id}")]
        public async Task<IActionResult> GetVotingQuestionByConference(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromRoute] int id,
            [FromQuery] string apikey)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                List<VotingQuestion> questions = this._context.VotingQuestion.Where(q => q.ConferenceID == conference_id).ToList();

                return this.Ok(questions);
            }

            return this.Unauthorized();
        }

        // Post: api/VotingQuestions/closeQuestion/5
        [HttpPost("closeQuestion/{id}")]
        public async Task<IActionResult> CloseVotingQuestion(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromRoute] int id,
            [FromQuery] string apikey)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var question = await this._context.VotingQuestion.FindAsync(id);

                if (question == null)
                {
                    return NotFound();
                }

                question.ResolvedOn = DateTime.Now;
                question.SumYes = 0;
                question.SumNo = 0;
                question.SumAbstention = 0;

                // select all votes for the question but only the highest priority per council
                List<VotingAnswer> answers = this._context.VotingAnswer
                    .Where(x => x.QuestionID == question.QuestionID)
                    .GroupBy(x => x.CouncilID)
                    .Select(group => group.OrderBy(vote => vote.Priority).First())
                    .ToList();
                answers.ForEach(x => this.AddVoteToQuestion(question, this.GetVoteType(x.Vote)));

                this._context.Entry(question).State = EntityState.Modified;
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.VotingQuestionExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(question);
            }

            return this.Unauthorized();
        }

        // GET: api/VotingQuestions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVotingQuestion(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromRoute] int id,
            [FromQuery] string apikey)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "user") && this.auth.KeyIsValid(apikey))
            {
                if (!ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var question = await _context.VotingQuestion.FindAsync(id);

                if (question == null)
                {
                    return NotFound();
                }

                VotingQuestionWithAnswers votingWithAnswers = new VotingQuestionWithAnswers(question);
                votingWithAnswers.CouncilAnswer = this._context.VotingAnswer
                    .Where(x => x.QuestionID == question.QuestionID && x.CouncilID == this.jwtService.GetCouncilfromJwtKey(jwttoken))
                    .OrderBy(x => x.Priority)
                    .FirstOrDefault();
                if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin"))
                {
                    var votingAnswers = await this._context.VotingAnswer.Where(x => x.QuestionID == question.QuestionID).ToListAsync();
                    if (votingAnswers != null)
                    {
                        votingAnswers = await this.AddCouncilToAnswers(votingAnswers);
                        votingWithAnswers.AnswerList = votingAnswers;
                    }
                }

                return Ok(votingWithAnswers);
            }

            return this.Unauthorized();
        }

        private async Task<List<VotingAnswer>> AddCouncilToAnswers(List<VotingAnswer> votingAnswers)
        {
            List<Task<Council>> listOfTaks = new List<Task<Council>>();
            foreach (var answer in votingAnswers)
            {
                listOfTaks.Add(this._context.Council.FindAsync(answer.CouncilID));
            }

            var councils = await Task.WhenAll<Council>(listOfTaks);
            for (int i = 0; i < councils.Length; i++)
            {
                votingAnswers[i].Council = councils[i];
            }
            return votingAnswers;
        }

        // PUT: api/VotingQuestions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVotingQuestion(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromRoute] int id,
            [FromBody] VotingQuestion votingQuestion)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                if (id != votingQuestion.QuestionID)
                {
                    return this.BadRequest();
                }

                this._context.Entry(votingQuestion).State = EntityState.Modified;

                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.VotingQuestionExists(id))
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

        // POST: api/VotingQuestions
        [HttpPost]
        public async Task<IActionResult> PostVotingQuestion(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromBody] VotingQuestion votingQuestion)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                this._context.VotingQuestion.Add(votingQuestion);
                await this._context.SaveChangesAsync();

                return this.CreatedAtAction("GetVotingQuestion", new { id = votingQuestion.QuestionID }, votingQuestion);
            }

            return this.Unauthorized();
        }

        // DELETE: api/VotingQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVotingQuestion(
            [FromHeader(Name = "conference_id")] int conference_id,
            [FromHeader(Name = "jwttoken")] string jwttoken,
            [FromQuery] string apikey,
            [FromRoute] int id)
        {
            if (this.jwtService.PermissionLevelValid(jwttoken, "superadmin") && this.auth.KeyIsValid(apikey))
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var votingQuestion = await this._context.VotingQuestion.FindAsync(id);
                if (votingQuestion == null)
                {
                    return this.NotFound();
                }

                this._context.VotingQuestion.Remove(votingQuestion);
                await this._context.SaveChangesAsync();

                return this.Ok(votingQuestion);
            }
            return this.Unauthorized();
        }

        private bool VotingQuestionExists(int id)
        {
            return this._context.VotingQuestion.Any(e => e.QuestionID == id);
        }

        private void AddVoteToQuestion(VotingQuestion question, VoteType voteType)
        {
            switch (voteType)
            {
                case VoteType.Yes: question.SumYes +=  1; break;
                case VoteType.No: question.SumNo += 1; break;
                case VoteType.Abstention: question.SumAbstention += 1; break;
                default: return;
            }
        }

        private VoteType GetVoteType(string vote)
        {
            switch (vote)
            {
                case "Ja": return VoteType.Yes;
                case "Nein": return VoteType.No;
                case "Enthaltung": return VoteType.Abstention;
                default: return VoteType.Other;
            }
        }
    }
}