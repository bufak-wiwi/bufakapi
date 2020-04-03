using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace BuFaKAPI.Models
{
    public class VotingQuestion
    {
        [Key]
        public int QuestionID { get; set; }

        public int ConferenceID { get; set; }

        public int MajorityID { get; set; }

        public string QuestionText { get; set; }

        public int ArrivedCouncilCount {get;set;}

        public bool IsOpen { get; set; }

        public string Vote { get; set; }

        public DateTime ResolvedOn { get; set; }

        public int SumYes { get; set; }

        public int SumNo { get; set; }

        public int SumAbstention { get; set; }

    }
}
