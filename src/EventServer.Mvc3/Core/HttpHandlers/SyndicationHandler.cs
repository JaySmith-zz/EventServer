namespace EventServer.Core.HttpHandlers
{
    using System.IO;
    using System.ServiceModel.Syndication;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;

    using EventServer.Core.Services;

    public class SyndicationHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var syndicationService = DependencyResolver.Current.GetService<ISyndicationService>();
            SyndicationFeed feed = syndicationService.GetFeed();

            var output = new StringWriter();
            var writer = new XmlTextWriter(output);

            var feedWritter = new Rss20FeedFormatter(feed);
            feedWritter.WriteTo(writer);

            context.Response.ContentType = "application/rss_xml";
            context.Response.AppendHeader("Content-Disposition", "inline; filename=rss.xml");
            context.Response.Write(output.ToString());
        }
    }
}