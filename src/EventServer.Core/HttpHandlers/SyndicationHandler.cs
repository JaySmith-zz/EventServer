using System.IO;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using EventServer.Core.Services;

namespace EventServer.Core.HttpHandlers
{
    public class SyndicationHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var syndicationService = Ioc.Resolve<ISyndicationService>();
            SyndicationFeed feed = syndicationService.GetFeed();

            var output = new StringWriter();
            var writer = new XmlTextWriter(output);

            var feedWritter = new Rss20FeedFormatter(feed);
            feedWritter.WriteTo(writer);

            context.Response.ContentType = "application/rss_xml";
            context.Response.AppendHeader("Content-Disposition", "inline; filename=rss.xml");
            context.Response.Write(output.ToString());
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}