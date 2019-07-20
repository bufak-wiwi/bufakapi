namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models;

    public class LoginResult
    {
        public string TokenString { get; set; }

        public User user { get; set; }

        public bool Applied { get; set; }

        public int Priority { get; set; }

        public bool Admin { get; set; }

        public int AdminForConference { get; set; }

        public List<Conference> Conferences { get; set; }
    }
}
