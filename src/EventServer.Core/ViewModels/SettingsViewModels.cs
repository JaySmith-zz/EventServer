namespace EventServer.Core.ViewModels
{
    using System;
    using System.ComponentModel;

    public class SettingsTwitterViewModel
    {
        [DisplayName("Event Twitter Account")]
        public string Id { get; set; }

        [DisplayName("Only show tweets since")]
        public DateTime FilterDate { get; set; }

        [DisplayName("Number of tweets to display")]
        public int DisplayCount { get; set; }
    }

    public class SettingsVenueViewModel
    {
        [DisplayName("Location")]
        public string VenueName { get; set; }

        [DisplayName("Location Phone")]
        public string VenuePhone { get; set; }

        [DisplayName("Address")]
        public string VenueStreet { get; set; }

        [DisplayName("City")]
        public string VenueCity { get; set; }

        [DisplayName("State")]
        public string VenueState { get; set; }

        [DisplayName("Postal Code")]
        public string VenueZip { get; set; }
    }
}