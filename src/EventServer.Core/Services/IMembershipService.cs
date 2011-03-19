using System;
using System.Web.Security;

namespace EventServer.Core.Services
{
    public interface IMembershipService
    {
        bool ValidateUser(string email, string password);
        string CreateUser(string email, string password);
        bool ChangePassword(string email, string oldPassword, string newPassword);
        void AddUserToRole(string email, string role);
        void RemoveUserFromRole(string email, string role);
        bool IsUserInRole(string email, string role);
        void Delete(string email);
    }

    public class MembershipService : IMembershipService
    {
        public bool ValidateUser(string email, string password)
        {
            ValidationUtil.ValidateRequiredStringValue(email, "email");
            ValidationUtil.ValidateRequiredStringValue(password, "password");

            return Membership.Provider.ValidateUser(email, password);
        }

        public string CreateUser(string email, string password)
        {
            ValidationUtil.ValidateRequiredStringValue(email, "email");
            ValidationUtil.ValidateRequiredStringValue(password, "password");

            MembershipCreateStatus status;
            Membership.Provider.CreateUser(null, password, email, null, null, true, null, out status);

            return MembershipCreateStatusToString(status);
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            ValidationUtil.ValidateRequiredStringValue(email, "email");
            ValidationUtil.ValidateRequiredStringValue(oldPassword, "oldPassword");
            ValidationUtil.ValidateRequiredStringValue(newPassword, "newPassword");

            try
            {
                MembershipUser currentUser = Membership.Provider.GetUser(email, true);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public void AddUserToRole(string email, string role)
        {
            Roles.Provider.AddUsersToRoles(new[] {email}, new[] {role});
        }

        public void RemoveUserFromRole(string email, string role)
        {
            Roles.Provider.RemoveUsersFromRoles(new[] {email}, new[] {role});
        }

        public bool IsUserInRole(string email, string role)
        {
            return Roles.Provider.IsUserInRole(email, role);
        }

        public void Delete(string email)
        {
            Membership.Provider.DeleteUser(email, true);
        }

        private string MembershipCreateStatusToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    return "";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Email already exists. Please enter a different email address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}