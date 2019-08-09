namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class Workshop
    {
        [Key]
        public int WorkshopID { get; set; }

        [ForeignKey("ConferenceID")]
        public int ConferenceID { get; set; }

        public Conference Conference { get; set; }

        public string Name { get; set; }

        public string NameShort { get; set; }

        public string Overview { get; set; }

        public int MaxVisitors { get; set; }

        public string Difficulty { get; set; }

        [ForeignKey("HostUID")]
        public string HostUID { get; set; }

        public string HostName { get; set; }

        public User User { get; set; }

        public string Place { get; set; }

        public string Start { get; set; }

        public int Duration { get; set; }

        public string MaterialNote { get; set; }

        public bool Invalid { get; set; }
    }
}
