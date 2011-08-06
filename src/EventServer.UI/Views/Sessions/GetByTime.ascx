<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SessionsGetByTrackModel>" %>

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

  <% foreach (var dayGroup in Model.Sessions.OrderBy(x => x.Day).GroupBy(x => x.Day)) { %>

    <h3> Sessions for Day <%= dayGroup.Key %></h3>

    <% foreach (var sessionGroup in Model.Sessions.Where(x => x.Day == dayGroup.Key).OrderBy(x => x.Slot).GroupBy(x => x.TimeSlot)){ %>
  
        <h4> <%= sessionGroup.Key %> </h4>

        <table class="sessions" border="0" cellpadding="0" cellspacing="0">
        <% if (Model.IsAdmin) { %>
        <col style="width: 20px;" />
        <% } %>
        <col style="width: 150px;" />
        <col />
        <col style="width: 75px;" />
        <thead>
            <tr>
                <% if (Model.IsAdmin) { %>
                <th>&nbsp;</th>
                <% } %>
                <th>Speaker</th>
                <th>Title</th>
                <th>Track</th>
                <th>Time</th>
                <th>Room</th>
                <th>Day</th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var session in sessionGroup)
               { %>
            <tr class="status_<%= session.Status %>">
                <% if (Model.IsAdmin) { %>
                <td>
                    <a href="<%= Html.BuildUrlFromExpression<SessionsController>(c => c.Edit(session.Id)) %>"><%= Html.Image("~/Content/Images/img03.gif", "Edit track and slot") %></a>
                </td>
                <% } %>
                <td><%= Html.ActionLink<SpeakersController>(c => c.Show(session.UserId, session.SpeakerUrlName), session.SpeakerName) %></td>
                <td width="100%"><%= Html.ActionLink<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()), session.Title) %></td>
                <td><%= session.Track %></td>
                <td><nobr><%= session.TimeSlot %></nobr></td>
                <td><nobr><%= session.Room %></nobr></td>
                <td><nobr><%= session.Day %></nobr></td>
            </tr>
            <% } %>
        </tbody>
    </table>

    <% } %>

    

<% } %>
