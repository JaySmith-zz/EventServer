using System;
using System.Linq;
using EventServer.Core.Services;

namespace EventServer.Core.Domain
{
    public class UserProfile : Entity
    {
        public UserProfile()
        {
        }

        public UserProfile(string email, string password)
        {
            Email = email;
            Password = password.Hash();
            CreationDate = DateTime.Now;
        }

        public UserProfile(string email, string password, string name) : this(email, password)
        {
            Name = name;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public SpeakerProfile SpeakerProfile { get; set; }
        public DateTime CreationDate { get; set; }

        public string UrlName
        {
            get { return (Name ?? Email).MakeUrlFriendly().ToLower(); }
        }

        public bool IsSpeaker
        {
            get { return SpeakerProfile != null; }
        }

        public void CompleteRegistration(string name, bool registeredByAdmin)
        {
            Name = name;
            Raise(new UserRegistered {UserId = Id, RegisteredByAdmin = registeredByAdmin});
        }

        public void UpdatePassword(string newPassword)
        {
            Password = newPassword.Hash();
        }

        public void UpdateEmail(string newEmail)
        {
            Email = newEmail;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateSpeakerProfile(string bio, Uri imageUrl, Uri blogUrl, bool isMvp, Uri mvpProfileUrl, bool travelAssistance)
        {
            var isNew = SpeakerProfile == null;

            if (isNew)
                SpeakerProfile = new SpeakerProfile();

            SpeakerProfile.Biography = bio;
            
            //if (imageUrl != null) SpeakerProfile.ImageUrl = imageUrl.ToString();
            SpeakerProfile.ImageUrl = imageUrl == null ? null : imageUrl.ToString();
            
            SpeakerProfile.BlogUrl = blogUrl == null ? null : blogUrl.ToString();
            SpeakerProfile.IsMvp = isMvp;
            SpeakerProfile.MvpProfileUrl = mvpProfileUrl == null ? null : mvpProfileUrl.ToString();

            if (SpeakerProfile.TravelAssistance != travelAssistance)
            {
                SpeakerProfile.TravelAssistance = travelAssistance;

                if (travelAssistance)
                    Raise(new TravelAssistanceRequested {UserId = Id});
                else
                    Raise(new TravelAssistanceCancelled {UserId = Id});
            }

            if (isNew)
                Raise(new SpeakerCreated {UserId = Id});
        }
    }

    public static class UserProfileExtensions
    {
        public static UserProfile GetBy(this IQueryable<UserProfile> source, string email)
        {
            return source.SingleOrDefault(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public static IQueryable<UserProfile> Speakers(this IQueryable<UserProfile> source)
        {
            return source.Where(x => x.SpeakerProfile != null);
        }

        public static string[] AdminEmails(this IQueryable<UserProfile> source)
        {
            var membershipService = Ioc.Resolve<IMembershipService>();
            return source
                .Where(x => membershipService.IsUserInRole(x.Email, "Admin"))
                .Select(x => x.Email)
                .ToArray();
        }
    }
}