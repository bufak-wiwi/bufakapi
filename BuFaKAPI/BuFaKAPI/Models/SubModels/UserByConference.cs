namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for Displaying just a User with an ID and a Name
    /// </summary>
    public class UserByConference
    {
        /// <summary>
        /// Gets or sets the ID of a User
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the Surname of a User
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Name of a User
        /// </summary>
        public string Name { get; set; }
    }
}
