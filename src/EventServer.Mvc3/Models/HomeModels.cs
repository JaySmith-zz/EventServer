namespace EventServer.Models
{
    using System.Web.Mvc;

    using EventServer.Core.Domain;

    public class HomeIndexModel
    {
        public Post[] Posts { get; set; }
        public Tweet[] Tweets { get; set; }
        public Sponsor[] Sponsors { get; set; }
        public ContentResult Content { get; set; }
    }
}