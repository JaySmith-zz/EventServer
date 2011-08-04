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
    
    .ScheduleDay 
    {
        font-weight: bold;
        font-size: large;
        padding: 1px 5px;
        text-align: left;
        border-bottom: 1px solid #ddd;
</style>

<table border="1">
    <% foreach (var day in Model.Sessions.OrderBy(x => x.Day).GroupBy(x => x.Day)) { %>
    <tr>
        <td class="ScheduleDay" colspan="6">Day <%= day.Key %></td>
    </tr>
    <% foreach (var track in day.OrderBy(x => x.Track).GroupBy(x => x.Track)) {%>
    <tr>
        <td width="20px">&nbsp;</td>
        <td colspan="5">Track: <%= track.Key %></td>
    </tr>
    <% foreach (var time in track.OrderBy(x => x.TimeSlot).GroupBy(x => x.TimeSlot)) { %>
    <tr>
        <td width="40px" colspan="3">&nbsp;</td>
        <td colspan="3">Time: <%= time.Key %></td>
    </tr>

    <% foreach (var session in time.OrderBy(x => x.TimeSlot)) { %>
    <tr>
        <td width="60px" colspan="4">&nbsp;</td>
        <td>
        <% if (Model.IsAdmin) { %>
        <a href="<%= Html.BuildUrlFromExpression<SessionsController>(c => c.Edit(session.Id)) %>"><%= Html.Image("~/Content/Images/img03.gif", "Edit track and slot") %></a>
        <% } %>
        <%= Html.ActionLink<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()), session.Title) %>
        </td>
        <td align="right"><%= Html.ActionLink<SpeakersController>(c => c.Show(session.UserId, session.SpeakerUrlName), session.SpeakerName) %></td>
    </tr>
    <tr>
        <td colspan="4">&nbsp;</td>
        <td colspan="2"><%= session.Description %></td>
    </tr>
    <tr><td colspan="6">&nbsp;</td></tr>
    <% } %>
    <% } %>
    <% } %>
    <% } %>
</table>

