namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Auth
    {
        [Key]
        public int TokenID { get; set; }

        public string ApiKey { get; set; }

        public string Note { get; set; }

        public string CreatedOn { get; set; }

        public string ValidUntil { get; set; }

        public int ConferenceID { get; set; }
    }
}
