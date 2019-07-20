namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models;

    public class PostConference
    {
        public Conference conference { get; set; }

        public string note { get; set; }

    }
}
