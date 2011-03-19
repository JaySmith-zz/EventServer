<%@ Page Language="C#" MasterPageFile="~/Views/EmailTemplates/Email.Master" Inherits="ViewPage<string>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    <%= Model %>

</asp:Content>
