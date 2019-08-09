namespace BuFaKAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppSettings
    {
        public string FirebaseApiKey { get; set; }

        public string JwtKey { get; set; }

        public int CurrentConferenceID { get; set; }
    }
}
