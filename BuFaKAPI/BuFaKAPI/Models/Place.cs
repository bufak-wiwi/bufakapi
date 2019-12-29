namespace BuFaKAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Place
    {
        [Key]
        public string PlaceID { get; set; }

        public string Name { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }
    }
}
