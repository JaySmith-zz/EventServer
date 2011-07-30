using System;
using System.Collections.Generic;

namespace EventServer.Core.Domain
{
    public class Page : Entity
    {
        public Page()
        {
        }

        public Page(string author, string title, string content, string description, string slug, bool isPublished)
        {
            Author = author;
            Title = title;
            Content = content;
            Description = description;
            Slug = slug;
            IsPublished = isPublished;
            DateCreated = DateTime.Now;
        }

        protected string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public Guid Parent { get; set; }
        public bool HasParentPage { get; set; }
        public bool HasChildPages { get; set; }
        public string Slug { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFrontPage { get; set; }
        public bool ShowInList { get; set; }
        public DateTime DateCreated { get; set; }

        public bool IsVisible
        {
            get { return IsAuthenticated || IsPublished; }
        }
        public string RelativeLink
        {
            get { return null; }
        }
        public Uri AbsoluteLink
        {
            get { return null; }
        }
    }


    public static class PageExtensions
    {
        public static Page GetSpecialPage(this IRepository repository, SpecialPage specialPage)
        {
            return repository.Get<Page>((int)specialPage) ?? repository.Save(_defaultPages[specialPage]);
        }

        private static readonly IDictionary<SpecialPage, Page> _defaultPages = new Dictionary<SpecialPage, Page>
            {
                {SpecialPage.HomeMessage, new Page("System", "Home Message", "<h2>Home Message</h2><p>To edit this content, edit the blog page titled \"Home Message\"</p>", "", "", true) {Id = (int)SpecialPage.HomeMessage}},
                {SpecialPage.AboutMessage, new Page("System", "About Message", "<h2>About Message</h2><p>To edit this content, edit the blog page titled \"About Message\"</p>", "", "", true) {Id = (int)SpecialPage.AboutMessage}},
                {SpecialPage.Sidebar, new Page("System", "Sidebar", "<ul><li><h2>Sidebar</h2>To edit this content, edit the blog page titled \"Sidebar\"</li></ul>", "", "", true) {Id = (int)SpecialPage.Sidebar}},
                {SpecialPage.SponsorInformation, new Page("System", "Sponsorship Information", "<h2>Sponsors</h2><p>To edit this content, edit the blog page titled \"Sponsorship Information\"</p>", "", "", true) {Id = (int)SpecialPage.SponsorInformation}}
            };
    }
}