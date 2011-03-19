<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<Presentation>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    The presentation <%= Html.FullActionLink<SessionsController>(c => c.Show(Model.Id, Model.UrlTitle), Model.Title) %> has been <span style="color: #d00;">rejected</span>

</asp:Content>
