using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI.Models
{
    public class News
    {
        [Key]
        public string NewsID { get; set; }

        [ForeignKey("ConferenceID")]

        public string ConferenceID { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string PictureLink { get; set; }

        public bool Important { get; set; }
    }
}
