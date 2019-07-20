namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WorkshopExport
    {
        public int Workshop_ID { get; set; }

        public string Name { get; set; }

        public string Start { get; set; }

        public string Place { get; set; }
    }
}
