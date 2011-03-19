<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<string[]>" %>

<asp:Content ContentPlaceHolderID="EmailBody" runat="server">

    <p>Email templates:</p>
    
    <% foreach (var template in Model) { %>

        <a href="<%= template %>"><%= template %></a><br />

    <% } %>

</asp:Content>
