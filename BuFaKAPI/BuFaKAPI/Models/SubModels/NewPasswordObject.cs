namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class NewPasswordObject
    {
        public string UID { get; set; }

        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
