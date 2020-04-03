using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace BuFaKAPI.Models
{
    public class VotingAnswer
    {
        [Key]
        public int AnswerID { get; set; }

        public int QuestionID { get; set; }

        public Council Council { get; set; }

        public int CouncilID { get; set; }

        public int Priority { get; set; }

        public string Vote { get; set; }
    }
}
