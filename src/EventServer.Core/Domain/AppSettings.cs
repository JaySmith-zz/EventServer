using System;

namespace EventServer.Core.Domain
{
    public class AppSettings : Entity
    {
        public AppSettings()
        {
            Id = 1;    
        }

        private string siteName;
        public string SiteName
        {
            get
            {
                if (string.IsNullOrEmpty(siteName))
                    siteName = "Event Server";

                return siteName;
            }
            set { siteName = value; }
        }

        private string siteSlogan;
        public string SiteSlogan
        {
            get
            {
                if (string.IsNullOrEmpty(siteSlogan))
                    siteSlogan = "Making events easy!";

                return siteSlogan;
            }
            set { siteSlogan = value; }
        }

        private string siteTheme;
        public string SiteTheme
        {
            get
            {
                if (string.IsNullOrEmpty(siteTheme))
                    siteTheme = "Default";

                return siteTheme;
            }
            set { siteTheme = value; }
        }

        private string description;
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(description))
                    description = "Event Server was created to make setting up sites for conference easy.";

                return description;
            }
            set { description = value; }
        }

        private DateTime startDateTime;
        public DateTime StartDateTime
        {
            get
            {
                if (startDateTime == DateTime.MinValue)
                    startDateTime = DateTime.Now.AddDays(60);

                return startDateTime;
            }
            set { startDateTime = value; }
        }

        private DateTime endDateTime;
        public DateTime EndDateTime
        {
            get
            {
                if (endDateTime == DateTime.MinValue)
                    DateTime.Now.AddDays(90);
                
                return endDateTime;
            }
            set { endDateTime = value; }
        }

        private DateTime registrationEndDateTime;
        public DateTime RegistrationEndDateTime
        {
            get
            {
                if (registrationEndDateTime == DateTime.MinValue)
                    registrationEndDateTime = DateTime.Now.AddDays(89);
                
                return registrationEndDateTime;
            }
            set { registrationEndDateTime = value; }
        }

        private DateTime sessionSubmissionEndDateTime;
        public DateTime SessionSubmissionEndDateTime
        {
            get
            {
                if (sessionSubmissionEndDateTime == DateTime.MinValue)
                    sessionSubmissionEndDateTime = DateTime.Now.AddDays(50);

                return sessionSubmissionEndDateTime;
            }
            set { sessionSubmissionEndDateTime = value; }
        }

        private string venueName;
        public string VenueName
        {
            get
            {
                if (string.IsNullOrEmpty(venueName))
                    venueName = "Event Venue";

                return venueName;
            }
            set { venueName = value; }
        }

        private string venuePhone;
        public string VenuePhone
        {
            get
            {
                if (string.IsNullOrEmpty(venuePhone))
                    venuePhone = "555 555-5555";
                
                return venuePhone;
            }
            set { venuePhone = value; }
        }

        private string venueStreet;
        public string VenueStreet
        {
            get
            {
                if (string.IsNullOrEmpty(venueStreet))
                    venueStreet = "Anywhere St.";

                return venueStreet;
            }
            set { venueStreet = value; }
        }

        private string venueCity;
        public string VenueCity
        {
            get
            {
                if (string.IsNullOrEmpty(venueCity))
                    venueCity = "Anytown";

                return venueCity;
            }
            set { venueCity = value; }
        }

        private string venueState;
        public string VenueState
        {
            get
            {
                if (string.IsNullOrEmpty(venueState))
                    venueState = "AR";

                return venueState;
            }
            set { venueState = value; }
        }

        private string venueZip;
        public string VenueZip
        {
            get
            {
                if (string.IsNullOrEmpty(venueZip))
                    venueZip = "12345";
                
                return venueZip;
            }
            set { venueZip = value; }
        }

        private string contactName;
        public string ContactName
        {
            get
            {
                if (string.IsNullOrEmpty(contactName))
                    contactName = "Event Contact";
                
                return contactName;
            }
            set { contactName = value; }
        }

        private string contactEmail;
        public string ContactEmail
        {
            get
            {
                if (string.IsNullOrEmpty(contactName))
                    contactEmail = "event@example.com";
                
                return contactEmail;
            }
            set { contactEmail = value; }
        }

        private string twitterId;
        public string TwitterId
        {
            get
            {
                if (string.IsNullOrEmpty(twitterId))
                    twitterId = "EventServer";

                return twitterId;
            }
            set { twitterId = value; }
        }

        private DateTime twitterFilterDate;
        public DateTime TwitterFilterDate
        {
            get 
            {
                if (twitterFilterDate == DateTime.MinValue)
                    return DateTime.Now.AddDays(-30);

                return twitterFilterDate; 
            }
            set { twitterFilterDate = value; }
        }

        private int twitterDisplayCount;
        public int TwitterDisplayCount
        {
            get
            {
                if (twitterDisplayCount == 0)
                    twitterDisplayCount = 5;

                return twitterDisplayCount;
            }
            set { twitterDisplayCount = value; }
        }

        private string siteLogoUri;
        public string SiteLogoUri
        {
            get
            {
                if (string.IsNullOrEmpty(siteLogoUri))
                    siteLogoUri = "http://eventserver.codeplex.com/images/logo.png";
                
                return siteLogoUri;
            }
            set { siteLogoUri = value; }
        }

        private string dataStoreBasePath;
        public string DataStorePath
        {
            get 
            {
                if (string.IsNullOrEmpty(dataStoreBasePath))
                    dataStoreBasePath = "~/App_Data";

                return dataStoreBasePath; 
            }
            set { dataStoreBasePath = value; }
        }

        public string FileDataStorePath
        {
            get { return DataStorePath + "/Files"; }
        }
    }
}
