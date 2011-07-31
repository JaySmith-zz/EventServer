using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace EventServer.Core.Domain.DomainEventHandlers
{
    using EventServer.Models;

    public class EmailHandler :
        IHandles<UserRegistered>,
        IHandles<SpeakerCreated>,
        IHandles<TravelAssistanceRequested>,
        IHandles<TravelAssistanceCancelled>,
        IHandles<SessionCreated>,
        IHandles<SessionChanged>,
        IHandles<SessionAccepted>,
        IHandles<SessionRejected>,
        IHandles<CommentAdded>
    {
        public EmailHandler(IRepository repository, IMailGateway mailGateway)
        {
            _repository = repository;
            _mailGateway = mailGateway;
        }

        private readonly IRepository _repository;
        private readonly IMailGateway _mailGateway;

        public void Handle(UserRegistered domainEvent)
        {
            _mailGateway
                .GetMailerWith("Welcome", GetEmailBody("WelcomeNewUser", domainEvent.UserId))
                .SendTo(_repository.Get<UserProfile>(domainEvent.UserId).Email);

            if (domainEvent.RegisteredByAdmin)
                return;

            _mailGateway
                .GetMailerWith("New user", GetEmailBody(domainEvent, domainEvent.UserId))
                .SendTo(_repository.Find<UserProfile>().AdminEmails());
        }

        public void Handle(SpeakerCreated domainEvent)
        {
            _mailGateway
                .GetMailerWith("New speaker", GetEmailBody(domainEvent, domainEvent.UserId))
                .SendTo(_repository.Find<UserProfile>().AdminEmails());
        }

        public void Handle(TravelAssistanceRequested domainEvent)
        {
            _mailGateway
                .GetMailerWith("Travel assistance requested", GetEmailBody(domainEvent, domainEvent.UserId))
                .SendTo(_repository.Find<UserProfile>().AdminEmails());
        }

        public void Handle(TravelAssistanceCancelled domainEvent)
        {
            _mailGateway
                .GetMailerWith("Travel assistance cancelled", GetEmailBody(domainEvent, domainEvent.UserId))
                .SendTo(_repository.Find<UserProfile>().AdminEmails());
        }

        public void Handle(SessionCreated domainEvent)
        {
            _mailGateway
                .GetMailerWith("Session created", GetEmailBody(domainEvent, domainEvent.PresentationId))
                .SendTo(_repository.Find<UserProfile>().AdminEmails());
        }

        public void Handle(SessionChanged domainEvent)
        {
            _mailGateway
                .GetMailerWith("Session changed", GetEmailBody(domainEvent, domainEvent.SessionId))
                .SendTo(_repository.Find<UserProfile>().AdminEmails());
        }

        public void Handle(SessionAccepted domainEvent)
        {
            var presentation = _repository.Get<Session>(domainEvent.SessionId);

            _mailGateway
                .GetMailerWith("Session accepted", GetEmailBody(domainEvent, domainEvent.SessionId))
                .AddRecipients(_repository.Find<UserProfile>().AdminEmails())
                .AddRecipients(_repository.Get<UserProfile>(presentation.UserId).Email)
                .Send();
        }

        public void Handle(SessionRejected domainEvent)
        {
            var presentation = _repository.Get<Session>(domainEvent.SessionId);

            _mailGateway
                .GetMailerWith("Session rejected", GetEmailBody(domainEvent, domainEvent.SessionId))
                .AddRecipients(_repository.Find<UserProfile>().AdminEmails())
                .AddRecipients(_repository.Get<UserProfile>(presentation.UserId).Email)
                .Send();
        }

        public void Handle(CommentAdded domainEvent)
        {
            var presentation = _repository.Get<Session>(domainEvent.PresentationId);

            _mailGateway
                .GetMailerWith("Session comment added", GetEmailBody(domainEvent, domainEvent.PresentationId))
                .AddRecipients(_repository.Find<UserProfile>().AdminEmails())
                .AddRecipients(_repository.Get<UserProfile>(presentation.UserId).Email)
                .Send();
        }

        private string GetEmailBody(IDomainEvent domainEvent, int id)
        {
            return GetEmailBody(domainEvent.GetType().Name, id, null);
        }

        private string GetEmailBody(string templateName, int id)
        {
            return GetEmailBody(templateName, id, null);
        }

        private string GetEmailBody(string templateName, int id, object extraRouteValues)
        {
            var routeValues = new RouteValueDictionary(new {id});

            if (extraRouteValues != null)
                new RouteValueDictionary(extraRouteValues).Each(x => routeValues[x.Key] = x.Value);

            return GetEmailBody(templateName, routeValues);
        }

        private string GetEmailBody(string templateName, RouteValueDictionary routeValues)
        {
            routeValues["controller"] = "EmailTemplates";
            routeValues["action"] = templateName;

            var baseUri = new Uri(SystemPath.BaseUri());
            var relativeUri = RouteTable.Routes.GetVirtualPath(new FakeRequestContext(), routeValues).VirtualPath;
            var address = new Uri(baseUri, relativeUri);

            try
            {
                using (var webClient = new WebClient())
                    return webClient.DownloadString(address);
            }
            catch (WebException ex)
            {
                if (!(ex.Response is HttpWebResponse))
                    return "<p>{0}</p><pre>{1}</pre>".F(address, HttpUtility.HtmlEncode(ex.ToString()));

                using (TextReader reader = new StreamReader(ex.Response.As<HttpWebResponse>().GetResponseStream()))
                    return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "<p>{0}</p><pre>{1}</pre>".F(address, HttpUtility.HtmlEncode(ex.ToString()));
            }
        }

        private class FakeRequestContext : RequestContext
        {
            public FakeRequestContext() : base(new FakeHttpContext(), new RouteData())
            {
            }

            private class FakeHttpContext : HttpContextBase
            {
                private HttpRequestBase _request;
                private HttpResponseBase _response;

                public override HttpRequestBase Request
                {
                    get { return _request ?? (_request = new FakeHttpRequest()); }
                }

                public override HttpResponseBase Response
                {
                    get { return _response ?? (_response = new FakeHttpResponse()); }
                }

                private class FakeHttpRequest : HttpRequestBase
                {
                    public override string ApplicationPath
                    {
                        get { return SystemPath.ApplicationPath(); }
                    }
                }

                private class FakeHttpResponse : HttpResponseBase
                {
                    public override string ApplyAppPathModifier(string virtualPath)
                    {
                        return virtualPath;
                    }
                }
            }
        }
    }
}