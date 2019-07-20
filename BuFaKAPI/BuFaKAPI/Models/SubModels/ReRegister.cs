namespace BuFaKAPI.Models.SubModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// InputClass for Reregistering a User for a conference
    /// </summary>
    public class ReRegister
    {
        public string OldUID { get; set; }

        public string NewUID { get; set; }

        public bool HasNewSensibles { get; set; }

        public Sensible NewSensible { get; set; }

        public bool HasNewPriority { get; set; }

        public int NewPriority { get; set; }
    }
}
