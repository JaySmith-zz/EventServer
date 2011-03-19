using System;

namespace EventServer.Core.Domain
{
    public class PresentationCategory : ValueObject
    {
        public static PresentationCategory Developers = new PresentationCategory("Developers");
        public static PresentationCategory BusinessAnalysts = new PresentationCategory("Business Analysts");
        public static PresentationCategory ProjectManagers = new PresentationCategory("Project Managers");
        public static PresentationCategory TechnologySupport = new PresentationCategory("Technology and Support");
        public static PresentationCategory SAP = new PresentationCategory("SAP");
        public static PresentationCategory IndustrialAutomation = new PresentationCategory("Industrial Automation");

        public PresentationCategory() : base(" ")
        {
        }

        private PresentationCategory(string value) : base(value)
        {
        }

        public static implicit operator PresentationCategory(string value)
        {
            return new PresentationCategory(value);
        }
    }
}