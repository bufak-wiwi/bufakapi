// <copyright file="Conference_ApplicationController.cs" company="BuFaKWiSo">
// Copyright (c) BuFaKWiSo. All rights reserved.
// </copyright>

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Council
    {
        [Key]
        public int CouncilID { get; set; }

        public string Name { get; set; }

        public string NameShort { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string University { get; set; }

        public string UniversityShort { get; set; }

        public string Address { get; set; }

        public string ContactEmail { get; set; }

        public bool Invalid { get; set; }
    }
}
