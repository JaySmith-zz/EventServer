<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<UserProfile>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    <%= Html.FullActionLink<SpeakersController>(c => c.Show(Model.Id, Model.UrlName), Model.Name) %> just created a speaker profile

</asp:Content>
