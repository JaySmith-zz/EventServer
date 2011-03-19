<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<Presentation>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    A comment was added to <%= Html.FullActionLink<SessionsController>(c => c.Show(Model.Id, Model.UrlTitle), Model.Title) %>

</asp:Content>
