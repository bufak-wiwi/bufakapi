namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models;

    public class UserForConference
    {
        public int Conference_ID { get; set; }

        public bool Applied { get; set; }

        public bool Admin { get; set; }

        public bool Attendee { get; set; }

        public bool Rejected { get; set; }

        public int Priority { get; set; }
    }

    public class LoginResult
    {
        public string TokenString { get; set; }

        public User User { get; set; }

        public List<UserForConference> UserForConference { get; set; }

        public List<Conference> Conferences { get; set; }
    }
}
