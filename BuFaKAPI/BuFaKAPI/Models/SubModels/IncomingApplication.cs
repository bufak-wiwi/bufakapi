namespace BuFaKAPI.Models.SubModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A Model for an Incoming Conference Application
    /// </summary>
    public class IncomingApplication
    {
        /// <summary>
        /// Gets or sets the ID of the Conference in Question
        /// </summary>
        public int ConferenceID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Applicant
        /// </summary>
        public string ApplicantUID { get; set; }

        /// <summary>
        /// Gets or sets the Priority of the Applicant
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is an Alumnus
        /// </summary>
        public bool IsAlumnus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is of the BuFaK Council
        /// </summary>
        public bool IsBuFaKCouncil { get; set; }

        /// <summary>
        /// Gets or sets the Users BuFaK Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the Sleeping Preferences of the User
        /// </summary>
        public string SleepingPref { get; set; }

        /// <summary>
        /// Gets or sets the Mobile Number of the User
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// Gets or sets the Eating Preferences of the User
        /// </summary>
        public string Eating { get; set; }

        /// <summary>
        /// Gets or sets the Intolerances of the User
        /// </summary>
        public string Intolerance { get; set; }

        /// <summary>
        /// Gets or sets an extra Note for the Conference Application
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the Hotel for the User
        /// </summary>
        public string Hotel { get; set; }

        /// <summary>
        /// Gets or sets the Room for the User
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Gets or sets the Status of the Conference Application
        /// 1: IsRejected
        /// 2: IsAttendee
        /// </summary>
        [EnumDataType(typeof(CAStatus))]
        public CAStatus Status { get; set; }
    }
}
