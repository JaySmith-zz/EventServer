namespace EventServer.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using EventServer.Core.Domain;

    public class SpeakersIndexModel
    {
        public UserProfile[] Speakers { get; set; }
    }

    public class SpeakersShowModel
    {
        public bool CanAddSession { get; set; }
        public UserProfile User { get; set; }
        public Session[] Sessions { get; set; }
    }

    [CustomValidator("IsMvpProfileUrlValid", "The MVP Profile Url field is required if Microsoft MVP is checked")]
    public class SpeakersEditModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int SpeakerId { get; set; }

        [Required]
        public string Bio { get; set; }

        [ValidateUrl]
        [DataType(DataType.Url)]
        [DisplayName("Profile Picture Url")]
        public Uri ImageUrl { get; set; }

        [ValidateUrl]
        [DataType(DataType.Url)]
        [DisplayName("Blog Url")]
        public Uri BlogUrl { get; set; }

        [DisplayName("Microsoft MVP")]
        public bool IsMvp { get; set; }

        [ValidateUrl]
        [DataType(DataType.Url)]
        [DisplayName("MVP Profile Url")]
        public Uri MvpProfileUrl { get; set; }

        [DisplayName("I would like to request travel assistance")]
        public bool TravelAssistance { get; set; }

        public bool IsMvpProfileUrlValid()
        {
            return !IsMvp || MvpProfileUrl != null;
        }
    }
}