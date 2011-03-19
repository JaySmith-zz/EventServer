using System;
using EventServer.Core.Domain;

namespace EventServer.Core.ViewModels
{
    public class AdminUsersModel
    {
        public UserModel[] Users { get; set; }
        public AccountRegisterModel NewUser { get; set; }

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
        public Presentation[] Sessions { get; set; }
    }
}