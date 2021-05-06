using System;
using System.ComponentModel.DataAnnotations;

namespace BuFaKAPI.Models
{
    public class VotingQuestion
    {
        [Key]
        public int QuestionID { get; set; }

        public int ConferenceID { get; set; }

        public int MajorityID { get; set; }

        public string QuestionText { get; set; }

        public int ArrivedCouncilCount { get; set; }

        public bool IsOpen { get; set; }

        public bool IsSecret { get; set; }

        public string Vote { get; set; }

        public DateTime? ResolvedOn { get; set; }

        public int SumYes { get; set; }

        public int SumNo { get; set; }

        public int SumAbstention { get; set; }

    }
}
