using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI.Models
{
    public class Event
    {
        [Key]
        public string EventID { get; set; }

        [ForeignKey("ConferenceID")]

        public string ConferenceID { get; set; }

        public string Start { get; set; }

        public int Duration { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        [ForeignKey("PlaceID")]

        public string PlaceID { get; set; }
    }
}
