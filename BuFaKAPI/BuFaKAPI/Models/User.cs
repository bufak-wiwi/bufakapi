namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class User
    {
        [Key]
        public string UID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Birthday { get; set; }

        public string Email { get; set; }

        public int CouncilID { get; set; }

        public string Address { get; set; }

        public string Sex { get; set; }

        public string Note { get; set; }

        public bool Invalid { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool PublicProfile { get; set; }

        public string AddFields { get; set; }
    }
}
