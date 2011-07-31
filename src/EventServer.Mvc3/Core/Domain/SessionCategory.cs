using System;

namespace EventServer.Core.Domain
{
    using EventServer.Models;

    public class SessionCategory : ValueObject
    {
        public static SessionCategory Developers = new SessionCategory("Developers");
        public static SessionCategory BusinessAnalysts = new SessionCategory("Business Analysts");
        public static SessionCategory ProjectManagers = new SessionCategory("Project Managers");
        public static SessionCategory TechnologySupport = new SessionCategory("Technology and Support");
        public static SessionCategory SAP = new SessionCategory("SAP");
        public static SessionCategory IndustrialAutomation = new SessionCategory("Industrial Automation");

        public SessionCategory() : base(" ")
        {
        }

        private SessionCategory(string value) : base(value)
        {
        }

        public static implicit operator SessionCategory(string value)
        {
            return new SessionCategory(value);
        }
    }
}