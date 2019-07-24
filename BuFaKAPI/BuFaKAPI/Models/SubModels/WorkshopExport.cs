namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for Exporting a sole Workshop
    /// </summary>
    public class WorkshopExport
    {
        /// <summary>
        /// Gets or sets the ID of the Workshop
        /// </summary>
        public int Workshop_ID { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Workshop
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Start date and time of the Workshop
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the Place of the Workshop
        /// </summary>
        public string Place { get; set; }
    }
}
