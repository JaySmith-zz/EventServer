using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventServer.Core.Domain;

namespace EventServer.Core.Services
{
    public interface ICurrentUserService
    {
        bool IsSignedIn { get; }
        bool IsAdmin { get; }
        string Email { get; }

        bool Is(string email);
        bool Is(UserProfile user);
        bool Owns(Presentation presentation);
        void AddRedirectMessage(string message);
        string[] GetAndFlushRedirectMessages();
    }

    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IRepository repository)
        {
            _repository = repository;

            var context = HttpContext.Current;

            IsSignedIn = context.Request.IsAuthenticated;
            IsAdmin = context.User.IsInRole("Admin");
            Email = !IsSignedIn ? "Guest" : context.User.Identity.Name;
        }

        private readonly IRepository _repository;

        public bool IsSignedIn { get; private set; }
        public bool IsAdmin { get; private set; }
        public string Email { get; private set; }

        public bool Is(string email)
        {
            return string.Equals(email, Email, StringComparison.OrdinalIgnoreCase);
        }

        public bool Is(UserProfile user)
        {
            return user != null && Is(user.Email);
        }

        public bool Owns(Presentation presentation)
        {
            if (presentation == null)
                return false;

            var user = _repository.Find<UserProfile>().GetBy(Email);

            return user != null && user.Id == presentation.UserId;
        }

        public void AddRedirectMessage(string message)
        {
            GetRedirectMessages().Add(message);
        }

        public string[] GetAndFlushRedirectMessages()
        {
            var list = GetRedirectMessages();
            var messages = list.ToArray();
            list.Clear();

            return messages;
        }

        private IList<string> GetRedirectMessages()
        {
            if (HttpContext.Current.Session["redirectMessages"] == null)
                HttpContext.Current.Session["redirectMessages"] = new List<string>();

            return HttpContext.Current.Session["redirectMessages"].As<IList<string>>();
        }
    }
}