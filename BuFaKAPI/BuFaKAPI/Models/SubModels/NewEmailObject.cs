namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for the changing of the E-Mail of a User
    /// </summary>
    public class NewEmailObject
    {
        public string UID { get; set; }

        public string OldEmail { get; set; }

        public string Password { get; set; }

        public string NewEmail { get; set; }
    }
}
