using System;

namespace EventServer.Core.Domain
{
    public class Sponsor : Entity
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public SponsorshipLevel Level { get; set; }
        public string LogoUri { get; set; }
    }

    public enum SponsorshipLevel
    {
        Gold = 2,
        Platinum = 4
    }
}