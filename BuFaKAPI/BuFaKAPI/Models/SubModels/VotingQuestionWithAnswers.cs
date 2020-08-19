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
            Vote = question.Vote;
            ResolvedOn = question.ResolvedOn;
            SumYes = question.SumYes;
            SumNo = question.SumNo;
            SumAbstention = question.SumAbstention;
        }
    }
}
