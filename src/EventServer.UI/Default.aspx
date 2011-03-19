<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>

<%-- Please do not delete this file. It is used to ensure that ASP.NET MVC is activated by IIS when a user makes a "/" request to the server. --%>

<script runat="server">
   protected void Page_Load(object sender, EventArgs e)
   {
      HttpContext.Current.RewritePath(Request.ApplicationPath, false);
      IHttpHandler httpHandler = new MvcHttpHandler();
      httpHandler.ProcessRequest(HttpContext.Current);
   }
</script>
