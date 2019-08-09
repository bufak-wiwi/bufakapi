namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The Model for a User with Password to register
    /// </summary>
    public class UserWithPassword
    {
        /// <summary>
        /// Gets or sets the ID of a User
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// Gets or sets the Name of a User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Surname of a User
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Birthday of a User
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// Gets or sets the E-Mail Address of a User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password of a User
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Council_ID of a User
        /// </summary>
        public int Council_id { get; set; }

        /// <summary>
        /// Gets or sets the Sex of a User
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets an additional Note for the User
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or set the personal Address of a User
        /// </summary>
        public string Address { get; set; }
    }
}
