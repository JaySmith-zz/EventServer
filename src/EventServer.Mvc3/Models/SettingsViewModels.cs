namespace EventServer.Models
{
    using System;
    using System.ComponentModel;

    public class TwitterSettingsViewModel
    {
        [DisplayName("Event Twitter Account")]
        public string Id { get; set; }

        [DisplayName("Only show tweets since")]
        public DateTime FilterDate { get; set; }

        [DisplayName("Number of tweets to display")]
        public int DisplayCount { get; set; }
    }

    public class EmailSettingsViewModel
    {
        [DisplayName("Enable Email")]
        public bool EmailEnabled { get; set; }

        [DisplayName("Email address")]
        public string EmailFromAddress { get; set; }

        [DisplayName("SMTP server")]
        public string EmailHost { get; set; }

        [DisplayName("Port number")]
        public int EmailHostPort { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Enable SSL")]
        public bool EnableSsl { get; set; }

        [DisplayName("Subject prefix")]
        public string SubjectPrefix { get; set; }
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