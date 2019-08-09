namespace BuFaKAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models;

    public class History
    {
        [Key]
        public int HistoryID { get; set; }

        public string OldValue { get; set; }

        [ForeignKey("ResponsibleUID")]
        public string ResponsibleUID { get; set; }

        public User User { get; set; }

        public string HistoryType { get; set; }
    }
}
