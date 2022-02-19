namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class Travel
    {
        [Key]
        public int TravelID { get; set; }

        public int ConferenceID { get; set; }

        public string UID { get; set; }

        public string Transportation { get; set; }

        public bool ParkingSpace { get; set; }

        public string ArrivalTimestamp { get; set; }

        public string ArrivalPlace { get; set; }

        public string DepartureTimestamp { get; set; }

        public string ExtraNote { get; set; }


    }
}
