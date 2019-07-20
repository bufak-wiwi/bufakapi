namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserWithPassword
    {
        public string Uid { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Birthday { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Council_id { get; set; }

        public string Sex { get; set; }

        public string Note { get; set; }

        public string Address { get; set; }
    }
}
