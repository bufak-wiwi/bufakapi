namespace BuFaKAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class Sensible
    {
        [Key]
        public int SensibleID { get; set; }

        [ForeignKey("ConferenceID")]
        public int ConferenceID { get; set; }

        public string Timestamp { get; set; }

        public int BuFaKCount { get; set; }

        [ForeignKey("UID")]
        public string UID { get; set; }

        public string EatingPreferences { get; set; }

        public string Intolerances { get; set; }

        public string SleepingPreferences { get; set; }

        public string Telephone { get; set; }

        public string ExtraNote { get; set; }

        public bool Invalid { get; set; }
    }
}
