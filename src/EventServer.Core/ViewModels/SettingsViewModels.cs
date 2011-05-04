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
}