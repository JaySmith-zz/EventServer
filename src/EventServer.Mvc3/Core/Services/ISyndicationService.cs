namespace EventServer.Core.Services
{
    using System.ServiceModel.Syndication;

    public interface ISyndicationService
    {
        SyndicationFeed GetFeed();
    }
}