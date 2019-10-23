namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for Changing the WA Status of multiple Workshop Applications
    /// </summary>
    public class ChangeWAStatusBulk
    {
        /// <summary>
        /// Gets or sets the User and Workshop to change the Status of
        /// </summary>
        public List<UserToWS> UsersToWorkshop { get; set; }

        /// <summary>
        /// Gets or sets the Status of the Workshop Application
        /// 0: HasApplied
        /// 1: IsRejected
        /// 2: IsPlanned
        /// 3: IsAttendee
        /// </summary>
        [EnumDataType(typeof(WAStatus))]
        public WAStatus NewStatus { get; set; }
    }

    /// <summary>
    /// Model for Matching the User and Workshop IDs
    /// </summary>
    public class UserToWS
    {
        /// <summary>
        /// Gets or sets the UserID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the WorkshopID
        /// </summary>
        public int WorkshopID { get; set; }
    }
}
