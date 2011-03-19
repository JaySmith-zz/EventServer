using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace EventServer.Core.HttpHandlers
{
    public class ImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["picture"]))
            {
                string fileName = context.Request.QueryString["picture"];

                try
                {
                    string folder = HostingEnvironment.MapPath("~/App_Data/Files");
                    FileInfo fi = new FileInfo(folder + Path.DirectorySeparatorChar + fileName);

                    if (fi.Exists && fi.Directory.FullName.ToUpperInvariant().Contains(Path.DirectorySeparatorChar + "FILES"))
                    {
                        context.Response.Cache.SetCacheability(HttpCacheability.Public);
                        context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));

                        //if (Utils.SetConditionalGetHeaders(fi.CreationTimeUtc))
                        //    return;

                        int index = fileName.LastIndexOf(".") + 1;
                        string extension = fileName.Substring(index).ToUpperInvariant();

                        // Fix for IE not handling jpg image types
                        if (string.Compare(extension, "JPG") == 0)
                            context.Response.ContentType = "image/jpeg";
                        else
                            context.Response.ContentType = "image/" + extension;

                        context.Response.TransmitFile(fi.FullName);
                    }
                    else
                    {
                        //context.Response.Redirect(Utils.AbsoluteWebRoot + "error404.aspx");
                    }
                }
                catch (Exception ex)
                {
                    //context.Response.Redirect(Utils.AbsoluteWebRoot + "error404.aspx");
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}