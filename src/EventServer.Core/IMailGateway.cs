using System;

namespace EventServer.Core
{
    public interface IMailGateway
    {
        IMailer GetMailerWith(string subject, string body);
    }

    public interface IMailer
    {
        IMailer SendTo(params string[] emailAddresses);
        IMailer AddRecipients(params string[] emailAddresses);
        IMailer Send();
    }
}