using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI.Models.SubModels
{
    public class LoginResult
    {
        public string TokenString { get; set; }

        public WebApplication1.Models.User user { get; set; }

        public bool Applied { get; set; }

        public int Priority { get; set; }

        public bool Admin { get; set; }
    }
}
