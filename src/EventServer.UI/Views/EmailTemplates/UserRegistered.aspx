<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<UserProfile>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    <%= Html.FullActionLink<AccountController>(c => c.Show(Model.Id, Model.UrlName), Model.Name) %> just registered

</asp:Content>
