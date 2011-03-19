using System;
using System.Collections.Generic;
using System.Linq;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using TweetSharp.Model;
using TweetSharp.Service;

namespace EventServer.Infrastructure.Services
{
    public class TweetSharpTwitterService : ITwitterService
    {
        public IEnumerable<Tweet> GetaRecentTweets()
        {
            //.Where(x => x.CreatedDate > Settings.Instance.TwitterFilterDate)

            var service =  new TwitterService();
            var items = new Tweet[] {};

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
                var item = new Tweet {Message = "Tweets currently unavailble!", PostDate = DateTime.Now};
                items = new Tweet[1] { item };
            }

            return items;
        }

        private static Tweet MapTwitterStatusToTweet(ITweetable status)
        {
            return new Tweet {PostDate = status.CreatedDate, Message = status.Text};
        }
    }
}