using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using EventServer.Core;
using EventServer.Core.Domain;

namespace EventServer.Infrastructure.Providers
{
    public class AppMembershipProvider : MembershipProvider
    {
        private readonly IRepository _repository = Ioc.Resolve<IRepository>();

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { return 3; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (GetUsers().Any(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            status = MembershipCreateStatus.Success;
            return Map(_repository.Save(new UserProfile(email, password)));
        }

        public override bool ChangePasswordQuestionAndAnswer(string email, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string email, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = GetUsers().Single(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));

            user.UpdatePassword(newPassword);
            _repository.Save(user);

            return true;
        }

        public override string ResetPassword(string email, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            var appUser = GetUsers().Single(x => x.Id == (int)user.ProviderUserKey);

            appUser.UpdateEmail(user.Email);
            _repository.Save(appUser);
        }

        public override bool ValidateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;

            return GetUsers()
                       .Where(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase))
                       .Where(x => string.Equals(x.Password, password.Hash()))
                       .Count() > 0;
        }

        public override bool UnlockUser(string email)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return Map(GetUsers().Single( x => x.Id == (int)providerUserKey));
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            return Map(GetUsers().Single(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase)));
        }

        public override string GetUserNameByEmail(string email)
        {
            return GetUsers().Single(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase)).Email;
        }

        public override bool DeleteUser(string email, bool deleteAllRelatedData)
        {
            var appUser = GetUsers().Single(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
            _repository.Delete(appUser);
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return FindUsers("", "", pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return FindUsers(usernameToMatch, "", pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return FindUsers("", emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        private UserProfile[] GetUsers()
        {
            var users = _repository.Find<UserProfile>().ToArray();

            if (users.Length == 0)
            {
                var user = new UserProfile("dev@dev.com", "dev", "Developer");
                _repository.Save(user);
                users = new[] {user};
            }

            return users;
        }

        private MembershipUser Map(UserProfile user)
        {
            return new MembershipUser(
                Name, // Provider name
                user.Email, // Username
                user.Id, // providerUserKey
                user.Email, // Email
                string.Empty, // passwordQuestion
                string.Empty, // Comment
                true, // isApproved
                false, // isLockedOut
                user.CreationDate, // creationDate
                DateTime.MinValue, // lastLoginDate
                DateTime.MinValue, // lastActivityDate
                user.CreationDate, // lastPasswordChangedDate
                DateTime.MinValue); // lastLockoutDate
        }

        public MembershipUserCollection FindUsers(string usernameToMatch, string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = GetUsers()
                .Where(x => string.IsNullOrEmpty(usernameToMatch) || Regex.IsMatch(x.Email, usernameToMatch, RegexOptions.IgnoreCase))
                .Where(x => string.IsNullOrEmpty(emailToMatch) || Regex.IsMatch(x.Email, emailToMatch, RegexOptions.IgnoreCase))
                .ToArray();

            totalRecords = users.Length;

            var membershipUsers = new MembershipUserCollection();

            users
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Each(x => membershipUsers.Add(Map(x)));

            return membershipUsers;
        }
    }
}