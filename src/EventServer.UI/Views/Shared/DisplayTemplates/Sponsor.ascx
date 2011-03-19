<%@ Control Language="C#" Inherits="ViewUserControl<Sponsor>" %>
<table border="0" cellpadding="3" cellspacing="3" width="100%">
    <tr>
        <td>
            <% if (string.IsNullOrEmpty(Model.Name))
               { %>
            <p>
                <%= Html.Image(Model.LogoUri, Model.Name)%></p>
            <% }
               else
               { %>
            <p>
                <a href="<%= Model.Url %>">
                    <%= Html.Image(Model.LogoUri, Model.Name, new {width = 100, border = 0}) %></a></p>
            <% } %>
        </td>
        <td width="100%">
            <%= Model.Description %>
        </td>
    </tr>
    <% if (Roles.IsUserInRole("Admin"))
       {%>
    <tr>
        <td align="right" colspan="2">
            <%
           if (Model.IsActive) { %>
                <%=Html.ActionLink("Inactivate", "Inactivate", new {id = Model.Id})%>
            <% } else { %>
                <%=Html.ActionLink("Activate", "Activate", new {id = Model.Id})%>
            <% } %>

            <%=Html.ActionLink("Edit", "Edit", new {id = Model.Id})%>
            <%=Html.ActionLink("Delete", "Delete", new {id = Model.Id})%>
        </td>
    </tr>
    <% } %>
</table>
