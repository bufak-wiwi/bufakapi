namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BulkApplication
    {
        public string Applicant_UID { get; set; }

        public int Workshop_ID { get; set; }

        public int Priority { get; set; }
    }
}
