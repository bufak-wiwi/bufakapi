namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// A Model for a Full Export of all Informations from a User
    /// </summary>
    public class CompleteExport
    {
        /// <summary>
        /// Gets or sets the UID of the to-be exported User
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the Surname of the User
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Name of the User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Email of the User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Birthday of the User
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// Gets or sets the Address of the User
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Sex of the User
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets the Council Object of the User
        /// </summary>
        public string Council { get; set; }

        /// <summary>
        /// Gets or sets the City of the Council of the User
        /// </summary>
        public string CouncilCity { get; set; }

        /// <summary>
        /// Gets or sets the State of the Council of the User
        /// </summary>
        public string CouncilState { get; set; }

        /// <summary>
        /// Gets or sets the Email of the Council of the User
        /// </summary>
        public string CouncilEmail { get; set; }

        /// <summary>
        /// Gets or sets the University of the Council of the User
        /// </summary>
        public string University { get; set; }

        /// <summary>
        /// Gets or sets the Address of the University of the Council of the User
        /// </summary>
        public string UniversityAddress { get; set; }

        /// <summary>
        /// Gets or sets the Note for the User
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the Priority of the User for the given Conference
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is Rejected or not
        /// </summary>
        public bool IsRejected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is of the BuFaK council or not
        /// </summary>
        public bool IsBuFaKCouncil { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is a Helper of some sorts
        /// </summary>
        public bool IsHelper { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is an Alumnus
        /// </summary>
        public bool IsAlumnus { get; set; }
    }
}
