<%@ Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Coming Soon
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <h2>Feature Coming soon!</h2>
   <p><%= Html.ActionLink<HomeController>(c => c.Index(), "home") %></p>
</asp:Content>
