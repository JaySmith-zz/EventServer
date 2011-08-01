using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventServer.Core.Domain;

namespace EventServer.Core.Services
{
    using EventServer.Models;

    public interface ICurrentUserService
    {
        bool IsSignedIn { get; }
        bool IsAdmin { get; }
        string Email { get; }

        bool Is(string email);
        bool Is(UserProfile user);
        bool Owns(Session session);
        void AddRedirectMessage(string message);
        string[] GetAndFlushRedirectMessages();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IRepository repository;

        public CurrentUserService(IRepository repository)
        {
            this.repository = repository;

            var context = HttpContext.Current;

            IsSignedIn = context.Request.IsAuthenticated;
            IsAdmin = context.User.IsInRole("Admin");
            Email = !IsSignedIn ? "Guest" : context.User.Identity.Name;
        }

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

        public bool Owns(Session session)
        {
            if (session == null)
            {
                return false;
            }

            var user = this.repository.Find<UserProfile>().GetBy(Email);

            return user != null && user.Id == session.UserId;
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

        private static IList<string> GetRedirectMessages()
        {
            if (HttpContext.Current.Session["redirectMessages"] == null)
            {
                HttpContext.Current.Session["redirectMessages"] = new List<string>();
            }

            return HttpContext.Current.Session["redirectMessages"].As<IList<string>>();
        }
    }
}