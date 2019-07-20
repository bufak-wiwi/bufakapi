namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class Workshop_Application
    {
        [ForeignKey("WorkshopID")]
        public int WorkshopID { get; set; }

        public Workshop Workshop { get; set; }

        [ForeignKey("ApplicantUID")]

        public string ApplicantUID { get; set; }

        public bool IsHelper { get; set; }

        public User User { get; set; }

        public int Priority { get; set; }

        public string Status { get; set; }

        public bool Invalid { get; set; }
    }
}
