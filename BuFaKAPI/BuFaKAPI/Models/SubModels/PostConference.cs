namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models;

    /// <summary>
    /// Model for posting a Conference with a Note
    /// </summary>
    public class PostConference
    {
        /// <summary>
        /// Gets or sets the Conference to Post
        /// </summary>
        public Conference Conference { get; set; }

        /// <summary>
        /// Gets or sets a Note to save to an apikey for example
        /// </summary>
        public string Note { get; set; }

    }
}
