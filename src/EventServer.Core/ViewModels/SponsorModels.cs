using System;
using EventServer.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace EventServer.Core.ViewModels
{
    public class SponsorIndexModel
    {
        public Sponsor[] PlatinumSponsors { get; set; }
        public Sponsor[] GoldSponsors { get; set; }
        public Sponsor[] InactiveSponsors { get; set; }
    }
}