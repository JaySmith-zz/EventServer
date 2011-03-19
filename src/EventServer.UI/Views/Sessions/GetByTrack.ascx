<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SessionsGetByTrackModel>" %>

<% if (Model.Presentations.Length == 0) { %>

    <p>Tracks not yet defined</p>
    <p>Please try again closer to the conference</p>

    <% return; %>

<% } %>

<style type="text/css">
    table.sessions {
        width: 100%;
        margin-bottom: 25px;
    }
    table.sessions th {
        font-weight: bold;
    }
    table.sessions th, table.sessions td {
        padding: 1px 5px;
        text-align: left;
        border-bottom: 1px solid #ddd;
    }
    table.sessions th, table.sessions td img {
        border: 0;
    }
</style>

<% if (Model.IsAdmin) { %>
    <p>Admin view</p>
<% } %>

<% foreach (var group in Model.Presentations.GroupBy(x => x.Track)) { %>

    <h3><%= group.Key %></h3>

    <table class="sessions" border="0" cellpadding="0" cellspacing="0">
        <% if (Model.IsAdmin) { %>
        <col style="width: 20px;" />
        <% } %>
        <col style="width: 150px;" />
        <col />
        <col style="width: 135px;" />
        <thead>
            <tr>
                <% if (Model.IsAdmin) { %>
                <th>&nbsp;</th>
                <% } %>
                <th>Speaker</th>
                <th>Title</th>
                <th>Time</th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var session in group) { %>
            <tr class="status_<%= session.Status %>">
                <% if (Model.IsAdmin) { %>
                <td>
                    <a href="<%= Html.BuildUrlFromExpression<SessionsController>(c => c.Edit(session.Id)) %>"><%= Html.Image("~/Content/Images/img03.gif", "Edit track and slot") %></a>
                </td>
                <% } %>
                <td><%= Html.ActionLink<SpeakersController>(c => c.Show(session.UserId, session.SpeakerUrlName), session.SpeakerName) %></td>
                <td><%= Html.ActionLink<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()), session.Title) %></td>
                <td><%= session.TimeSlot %></td>
            </tr>
            <% } %>
        </tbody>
    </table>

<% } %>
