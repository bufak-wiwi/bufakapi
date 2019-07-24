namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The Model for exporting infos for a Workshop Application
    /// </summary>
    public class WSApplication
    {
        /// <summary>
        /// Gets or sets the ID of a Workshop
        /// </summary>
        public int Workshop_ID { get; set; }

        /// <summary>
        /// Gets or sets the Name of a Workshop
        /// </summary>
        public string Workshop_Name { get; set; }

        /// <summary>
        /// Gets or sets the Name of a Workshop
        /// </summary>
        public string Workshop_Hostname { get; set; }

        /// <summary>
        /// Gets or sets the overall number of Applications for a Workshop
        /// </summary>
        public int Applications { get; set; }

        /// <summary>
        /// Gets or sets the number of Applicants with Priority 1 for this Workshop
        /// </summary>
        public int Applications_Prio_1 { get; set; }

        /// <summary>
        /// Gets or sets the number of Applicants with Priority 2 for this Workshop
        /// </summary>
        public int Applications_Prio_2 { get; set; }

        /// <summary>
        /// Gets or sets the number of Applicants with Priority 3 for this Workshop
        /// </summary>
        public int Applications_Prio_3 { get; set; }
    }
}
