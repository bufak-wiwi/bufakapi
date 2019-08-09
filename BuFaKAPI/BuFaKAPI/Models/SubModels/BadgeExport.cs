namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for a Export of all Informations given on a Badge
    /// </summary>
    public class BadgeExport
    {
        /// <summary>
        /// Gets or sets the ID of the User
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the Name of the User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Surname of the User
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Council of the User
        /// </summary>
        public int CouncilID { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Council of the User
        /// </summary>
        public string CouncilName { get; set; }

        /// <summary>
        /// Gets or sets the University of the User
        /// </summary>
        public string University { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is a Helper of some sorts
        /// </summary>
        public bool IsHelper { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is an Alumnus
        /// </summary>
        public bool IsAlumnus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is of the BuFaK Council
        /// </summary>
        public bool IsBuFaKCouncil { get; set; }

        /// <summary>
        /// Gets or sets a List of Workshops for a User
        /// </summary>
        public List<WorkshopExport> Workshops { get; set; }
    }
}
