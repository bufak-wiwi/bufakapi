namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// InputClass for Reregistering a User for a conference
    /// </summary>
    public class ReRegister
    {
        /// <summary>
        /// Gets or sets the Old UID of an Applicant
        /// </summary>
        public string OldUID { get; set; }

        /// <summary>
        /// Gets or sets the New UID of an Applicant
        /// </summary>
        public string NewUID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the new Applicant has new Sensible Data
        /// </summary>
        public bool HasNewSensibles { get; set; }

        /// <summary>
        /// Gets or sets the new SensibleData, if there is any to be inserted
        /// </summary>
        public Sensible NewSensible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the new Applicant has a new Priority
        /// </summary>
        public bool HasNewPriority { get; set; }

        /// <summary>
        /// Gets or sets the new Priority, if there is any to be inserted
        /// </summary>
        public int NewPriority { get; set; }
    }
}
