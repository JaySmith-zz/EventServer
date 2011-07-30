using System.Text;
using System.Web;
using System.Xml;

namespace EventServer.Core.HttpHandlers
{
    /// <summary>
    /// RSD (Really Simple Discoverability) Handler
    /// http://cyber.law.harvard.edu/blogs/gems/tech/rsd.html
    /// </summary>
    public class RsdHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            using (var rsd = new XmlTextWriter(context.Response.OutputStream, Encoding.UTF8))
            {
                rsd.Formatting = Formatting.Indented;
                rsd.WriteStartDocument();

                // Rsd tag
                rsd.WriteStartElement("rsd");
                rsd.WriteAttributeString("version", "1.0");

                // Service 
                rsd.WriteStartElement("service");
                rsd.WriteElementString("engineName", "EventServer "); // + BlogSettings.Instance.Version());
                rsd.WriteElementString("engineLink", "http://eventserver.codeplex.com");
                rsd.WriteElementString("homePageLink", AbsolutePath(context));
                    
                // APIs
                rsd.WriteStartElement("apis");

                // MetaWeblog
                rsd.WriteStartElement("api");
                rsd.WriteAttributeString("name", "MetaWeblog");
                rsd.WriteAttributeString("preferred", "true");
                string prefix = "http://";
                rsd.WriteAttributeString("apiLink",
                                         prefix + context.Request.Url.Authority + VirtualPathUtility.ToAbsolute("~/") + "metaweblog.axd");
                rsd.WriteAttributeString("blogID", AbsolutePath(context));
                rsd.WriteEndElement();

                // End APIs
                rsd.WriteEndElement();

                // End Service
                rsd.WriteEndElement();

                // End Rsd
                rsd.WriteEndElement();

                rsd.WriteEndDocument();
            }
        }

        public string AbsolutePath(HttpContext context)
        {
            var path = context.Request.Url.AbsoluteUri.Replace(context.Request.Url.AbsolutePath, null);
            if (path.EndsWith("/"))
                return path;
            else 
                return path + "/";
         
        }
    }
}