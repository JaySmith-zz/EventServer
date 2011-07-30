using System;
using System.Web.Security;

namespace EventServer.Core.Services
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string email, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string email, bool createPersistentCookie)
        {
            ValidationUtil.ValidateRequiredStringValue(email, "email");
            FormsAuthentication.SetAuthCookie(email, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}