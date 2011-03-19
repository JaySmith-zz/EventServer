using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;

namespace EventServer.Infrastructure.Services
{
    public class SyndicationService : ISyndicationService
    {
        public SyndicationFeed GetFeed()
        {
            var feed = new SyndicationFeed
            {
                //Title = new TextSyndicationContent(Settings.Instance.SiteName),
                //Description = new TextSyndicationContent(Settings.Instance.SiteSlogan),
                //ImageUrl = new Uri(Settings.Instance.SiteLogoUri),

                Title = new TextSyndicationContent("Tyson Development Conference"),
                ImageUrl = new Uri("http://tysondevcon.com/2010/Content/Images/logo.jpg"),
                
                Items = GetSyndicationItems()
            };

            return feed;
        }

        private static IEnumerable<SyndicationItem> GetSyndicationItems()
        {
            var repository = Ioc.Resolve<IRepository>();
            var posts = repository.Find<Post>(x => x.DateCreated <= DateTime.Now)
                .OrderByDescending(x => x.DateCreated);

            return posts.Select(post => ConvertPostToSyndicationItem(post)).ToList();
        }

        private static SyndicationItem ConvertPostToSyndicationItem(Post post)
        {
            var item = new SyndicationItem();

            item.Id = item.Id;
            item.Title = new TextSyndicationContent(post.Title);
            item.Content = new TextSyndicationContent(post.Content);
            item.PublishDate = post.DateCreated;

            return item;
        }
    }
}