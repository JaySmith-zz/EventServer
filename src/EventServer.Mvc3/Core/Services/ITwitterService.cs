using System.Collections.Generic;
using EventServer.Core.Domain;

namespace EventServer.Core.Services
{
    public interface ITwitterService
    {
        IEnumerable<Tweet> GetaRecentTweets();
    }
}