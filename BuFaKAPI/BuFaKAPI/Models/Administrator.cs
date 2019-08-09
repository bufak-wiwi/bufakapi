namespace BuFaKAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models;

    public class Administrator
    {
        [ForeignKey("uid")]
        public string UID { get; set; }

        public User User { get; set; }

        [ForeignKey("Conference_id")]
        public int ConferenceID { get; set; }

        public Conference Conference { get; set; }

        public string ValidUntil { get; set; }
    }
}
