namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WSApplication
    {
        public int Workshop_ID { get; set; }

        public string Workshop_Name { get; set; }

        public string Workshop_Hostname { get; set; }

        public int Applications { get; set; }

        public int Applications_Prio_1 { get; set; }

        public int Applications_Prio_2 { get; set; }

        public int Applications_Prio_3 { get; set; }
    }
}
