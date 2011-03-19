using System.ServiceModel.Syndication;

namespace EventServer.Core.Services
{
    public interface ISyndicationService
    {
        SyndicationFeed GetFeed();
    }
}