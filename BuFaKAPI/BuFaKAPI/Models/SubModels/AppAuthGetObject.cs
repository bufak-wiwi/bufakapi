using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace BuFaKAPI.Models.SubModels
{
    public class AppAuthGetObject
    {
        public int ID { get; set; }

        public int Conference_ID { get; set; }

        public Council Council { get; set; }

        public int Priority { get; set; }

        public string Password { get; set; }
    }
}
