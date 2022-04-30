namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using BuFaKAPI.Models;

    public class Conference_Application
    {
        /// <summary>
        /// Gets or sets the ID of the Conference
        /// </summary>
        [ForeignKey("Conference_id")]
        public int ConferenceID { get; set; }

        /// <summary>
        /// Gets or sets the Conference Object
        /// </summary>
        public Conference Conference { get; set; }

        /// <summary>
        /// Gets or sets the UID of the Applicant
        /// </summary>
        [ForeignKey("Applicant_uid")]
        public string ApplicantUID { get; set; }

        /// <summary>
        /// Gets or sets the User-Object of the Applicant
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the ID of the corresponding Sensible Object
        /// </summary>
        [ForeignKey("SensibleID")]
        public int SensibleID { get; set; }

        /// <summary>
        /// Gets or sets the corresponding Sensible Object
        /// </summary>
        public Sensible Sensible { get; set; }

        /// <summary>
        /// Gets or sets the Priority of the Conference Application
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Applicant is an Alumnus or not
        /// </summary>
        public bool IsAlumnus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Applicant is of the BuFaK Council or not
        /// </summary>
        public bool IsBuFaKCouncil { get; set; }

        /// <summary>
        /// Gets or sets an additional Note for the Conference Application
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets a Timestamp for the Conference Application
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Applicant is a Helper of some sorts
        /// </summary>
        public bool IsHelper { get; set; }

        /// <summary>
        /// Gets or sets the Hotel for the Applicant
        /// </summary>
        public string Hotel { get; set; }

        /// <summary>
        /// Gets or sets the Room for the Applicant
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Gets or sets a Status for the Conference Application
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dataset is invalid or not
        /// </summary>
        public bool Invalid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Applicant is of the BuFaK Council or not
        /// </summary>
        public bool IsAllowedToVote { get; set; }
    }
}
