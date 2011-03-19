<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SpeakersShowModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.User.Name %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.DisplayFor(m => m.User) %>

    <h2>Sessions</h2>

    <% if (Model.CanAddSession) { %>

        <p><%= Html.ActionLink<SessionsController>(c => c.Add(), "add session") %></p>

    <% } %>

    <% foreach (var session in Model.Sessions) { %>

        <%= Html.DisplayFor(m => session) %>

    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script language="javascript" type="text/javascript">

        $(function () {
            $("#navSpeakers").addClass("active");
        });

    </script>
</asp:Content>