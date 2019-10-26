namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WorkshopList
    {

        public int WorkshopID { get; set;}

        public string Name { get; set; }

        public string HostName { get; set; }

        public string Place { get; set; }

        public string Start { get; set; }

        public int MaxVisitors { get; set; }

        public string Overview { get; set; }

        public string MaterialNote { get; set; }

        public int Applicants { get; set; }

    }
}
