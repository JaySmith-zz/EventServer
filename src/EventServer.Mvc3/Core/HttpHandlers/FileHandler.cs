using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace EventServer.Core.HttpHandlers
{
    public class FileHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["file"]))
            {
                string fileName = context.Request.QueryString["file"];

                try
                {
                    string folder = HostingEnvironment.MapPath("~/App_Data/files");
                    FileInfo info = new FileInfo(folder + Path.DirectorySeparatorChar + fileName);

                    if (info.Exists && info.Directory.FullName.StartsWith(folder, StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.AppendHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
                        SetContentType(context, fileName);

                        //if (Utils.SetConditionalGetHeaders(info.CreationTimeUtc))
                        //    return;

                        context.Response.TransmitFile(info.FullName);
                    }
                    else
                    {
                        //context.Response.Redirect(Utils.AbsoluteWebRoot + "error404.aspx");
                    }
                }
                catch (Exception)
                {
                    //context.Response.Redirect(Utils.AbsoluteWebRoot + "error404.aspx");
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Sets the content type depending on the filename's extension.
        /// </summary>
        private static void SetContentType(HttpContext context, string fileName)
        {
            if (fileName.EndsWith(".pdf"))
                context.Response.AddHeader("Content-Type", "application/pdf");
            else
                context.Response.AddHeader("Content-Type", "application/octet-stream");
        }
    }
}