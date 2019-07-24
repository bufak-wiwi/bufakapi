namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for a Password-Change of a User
    /// </summary>
    public class NewPasswordObject
    {
        /// <summary>
        /// Gets or sets the ID of the User
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the E-Mail Address of the User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Old Password of the User
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the New Pasword of the User
        /// </summary>
        public string NewPassword { get; set; }
    }
}
