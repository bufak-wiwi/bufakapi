using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI.Models
{
    public class Newsletter
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Studyplace { get; set; }

        public string Sex { get; set; }

    }
}
