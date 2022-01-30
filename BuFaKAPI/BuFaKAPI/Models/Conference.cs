// <copyright file="Conference_ApplicationController.cs" company="BuFaKWiSo">
// Copyright (c) BuFaKWiSo. All rights reserved.
// </copyright>

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class Conference
    {
        [Key]
        public int ConferenceID { get; set; }

        public string Name { get; set; }

        public string DateStart { get; set; }

        public string DateEnd { get; set; }

        public int CouncilID { get; set; }

        public bool Invalid { get; set; }

        public bool ConferenceApplicationPhase { get; set; }

        public bool WorkshopApplicationPhase { get; set; }

        public bool WorkshopSuggestionPhase { get; set; }

        public string AttendeeCost { get; set; }

        public string AlumnusCost { get; set; }

        public string InformationTextConferenceApplication { get; set; }

        public string InformationTextWorkshopSuggestion { get; set; }

        public string LinkParticipantAgreement { get; set; }

        public string WorkshopDurations { get; set; }

        public string WorkshopTopics { get; set; }

        public string TravelArrivalPlaces { get; set; }

        public string AddFields { get; set; }
    }
}
