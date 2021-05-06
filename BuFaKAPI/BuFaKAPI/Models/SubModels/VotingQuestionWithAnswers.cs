using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI.Models.SubModels
{
    public class VotingQuestionWithAnswers : VotingQuestion
    {
        public List<VotingAnswer> AnswerList { get; set; } = new List<VotingAnswer>();

        public VotingAnswer CouncilAnswer { get; set; }

        public int TotalVotes { get; set; }

        public VotingQuestionWithAnswers(VotingQuestion question)
        {
            QuestionID = question.QuestionID;
            ConferenceID = question.ConferenceID;
            MajorityID = question.MajorityID;
            QuestionText = question.QuestionText;
            ArrivedCouncilCount = question.ArrivedCouncilCount;
            IsOpen = question.IsOpen;
            IsSecret = question.IsSecret;
            Vote = question.Vote;
            ResolvedOn = question.ResolvedOn;
            SumYes = ShowVote(question) ? question.SumYes : 0;
            SumNo = ShowVote(question) ? question.SumNo : 0;
            SumAbstention = ShowVote(question) ? question.SumAbstention : 0;
        }

        private static bool ShowVote(VotingQuestion question)
        {
            return question.ResolvedOn != null;
        }
    }
}
