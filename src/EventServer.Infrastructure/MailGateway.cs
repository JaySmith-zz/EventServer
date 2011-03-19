using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using EventServer.Core;

namespace EventServer.Infrastructure
{
    public class MailGateway : IMailGateway
    {
        public MailGateway(string developerEmail)
        {
            _developerEmail = developerEmail;
            _useRealEmail =
                string.IsNullOrEmpty(_developerEmail) ||
                string.Equals(_developerEmail, "YOUR.EMAIL@DOMAIN.COM", StringComparison.OrdinalIgnoreCase);
        }

        private readonly string _developerEmail;
        private readonly bool _useRealEmail;

        public IMailer GetMailerWith(string subject, string body)
        {
            return new Mailer(subject, body, _developerEmail, _useRealEmail);
        }

        private class Mailer : IMailer
        {
            public Mailer(string subject, string body, string developerEmail, bool useRealEmail)
            {
                _smtpClient = new SmtpClient();
                _smtpClient.EnableSsl = _smtpClient.Port != 25;

                _message = new MailMessage {Subject = subject, Body = body, IsBodyHtml = true};
                _originalBody = body;

                _developerEmail = developerEmail;
                _useRealEmail = useRealEmail;
            }

            private readonly SmtpClient _smtpClient;
            private readonly MailMessage _message;
            private readonly string _originalBody;
            private readonly string _developerEmail;
            private readonly bool _useRealEmail;
            private readonly IList<string> _recipients = new List<string>();

            public IMailer AddRecipients(params string[] emailAddresses)
            {
                emailAddresses.NullCheck().Each(email => _recipients.Add(email));

                return this;
            }

            public IMailer Send()
            {
                var emailAddresses = _recipients.ToArray();
                _recipients.Clear();

                return SendTo(emailAddresses);
            }

            public IMailer SendTo(params string[] emailAddresses)
            {
                foreach (var email in emailAddresses.Select(x => x.ToLower()).Distinct())
                {
                    try
                    {
                        _message.To.Clear();

                        if (_useRealEmail)
                            _message.To.Add(email);
                        else
                        {
                            _message.To.Add(_developerEmail);
                            _message.Body = "<p>Original recipient: {0}</p>{1}".F(email, _originalBody);
                        }

                        _smtpClient.Send(_message);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                return this;
            }
        }
    }
}