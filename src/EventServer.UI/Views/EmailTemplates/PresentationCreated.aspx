<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<Session>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    The presentation <%= Html.FullActionLink<SessionsController>(c => c.Show(Model.Id, Model.UrlTitle), Model.Title) %> has been created

</asp:Content>
