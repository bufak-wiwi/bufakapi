using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI.Models
{
    public class VotingMajority
    {
        [Key]
        public int MajorityID { get; set; }

        public string Secret { get; set; }

        public string Calculation { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

    }
}
