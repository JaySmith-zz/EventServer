<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<UserProfile>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    <%= Html.FullActionLink<SpeakersController>(c => c.Show(Model.Id, Model.UrlName), Model.Name) %> no longer needs travel assistance

</asp:Content>
