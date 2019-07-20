namespace BuFaKAPI.Models
{
    using System.ComponentModel;

    /// <summary>
    /// Enum for the used Status of a Conference Application
    /// </summary>
    public enum CAStatus
    {
        /// <summary>
        /// The Status of the Conference Application is at "HasApplied"
        /// </summary>
        [Description("HasApplied")]
        HasApplied,

        /// <summary>
        /// The Status of the Conference Application is at "IsRejected"
        /// </summary>
        [Description("IsRejected")]
        IsRejected,

        /// <summary>
        /// The Status of the Conference Application is at "IsAttendee"
        /// </summary>
        [Description("IsAttendee")]
        IsAttendee
    }
}
