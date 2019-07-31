namespace BuFaKAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model that shows the Passwords for every Priority of Every Council for a specific Conference
    /// </summary>
    public class ApplicationAuth
    {

        /// <summary>
        /// Gets or sets the ID of the Data
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the Conference in Question
        /// </summary>
        public int Conference_ID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Council in Question
        /// </summary>
        public int Council_ID { get; set; }

        /// <summary>
        /// Gets or sets the Priority inside of said Council
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the Password for the combination of the Values above
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Password is already used
        /// </summary>
        public bool Used { get; set; }
    }
}
