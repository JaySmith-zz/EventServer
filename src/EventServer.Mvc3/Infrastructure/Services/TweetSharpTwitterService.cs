namespace EventServer.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EventServer.Core;
    using EventServer.Core.Domain;
    using EventServer.Models;

    using TweetSharp;

    using ITwitterService = EventServer.Core.Services.ITwitterService;

    public class TweetSharpTwitterService : ITwitterService
    {
        public IEnumerable<Tweet> GetaRecentTweets()
        {
            var service = new TwitterService();
            Tweet[] items;

            try
            {
                items = service
                .ListTweetsOnSpecifiedUserTimeline(Settings.Instance.TwitterId, 0, Settings.Instance.TwitterDisplayCount)
                .NullCheck()
                .Select(MapTwitterStatusToTweet)
                .ToArray();
            }
            catch
            {
                var item = new Tweet { Message = "Tweets currently unavailble!", PostDate = DateTime.Now };
                items = new[] { item };
            }

            return items;
        }

        private static Tweet MapTwitterStatusToTweet(ITweetable status)
        {
            return new Tweet { PostDate = status.CreatedDate, Message = status.Text };
        }
    }
}