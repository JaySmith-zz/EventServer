namespace EventServer.Models
{
    using EventServer.Core.Domain;

    public class AdminUsersModel
    {
        public UserModel[] Users { get; set; }
        public RegisterModel NewUser { get; set; }

        public class UserModel
        {
            public UserProfile User { get; set; }
            public bool IsAdmin { get; set; }

            public bool RequestingTravelAssistance
            {
                get { return User.SpeakerProfile != null && User.SpeakerProfile.TravelAssistance; }
            }
        }
    }

    public class AdminSessionsModel
    {
        public Session[] Sessions { get; set; }
    }
}