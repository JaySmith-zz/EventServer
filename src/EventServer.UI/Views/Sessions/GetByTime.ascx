<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SessionsGetByTrackModel>" %>

<style type="text/css">
    .ScheduleDay 
    {
        font-weight: bold;
        font-size: large;
        padding: 1px 5px;
        text-align: left;
        border-bottom: 1px solid #ddd;
    }
    
    .ScheduleTitle
    {
        font-size: medium;
        font-weight: bold;
    }
</style>

<br /><br />
<% if (Model.Sessions.Count() > 0) { %>	
  
<table border="0" cellpadding="5">
    <% foreach (var day in Model.Sessions.OrderBy(x => x.Day).GroupBy(x => x.Day)) { %>
    <% foreach (var time in day.OrderBy(x => x.TimeSlot).GroupBy(x => x.TimeSlot)) {%>
    <tr>
        <td class="ScheduleDay" colspan="5">Day <%= day.Key %> - <%= time.Key %></td>
    </tr>
    <% foreach (var session in time.OrderBy(x => x.Title)) { %>
    <tr>
        <td width="60px">&nbsp;</td>
        <td colspan="4" class="ScheduleTitle">
        <% if (Model.IsAdmin) { %>
        <a href="<%= Html.BuildUrlFromExpression<SessionsController>(c => c.Edit(session.Id)) %>"><%= Html.Image("~/Content/Images/img03.gif", "Edit track and slot") %></a>
        <% } %>
        <%= Html.ActionLink<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()), session.Title) %>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>Speaker: <%= Html.ActionLink<SpeakersController>(c => c.Show(session.UserId, session.SpeakerUrlName), session.SpeakerName) %></td>
        <td>Track: <%= session.Track %></td>
        <td>Level: <%= session.Level %></td>
        <td>Room: <%= session.Room %></td>
    </tr>
    <tr>
        <td width="60px">&nbsp;</td>
        <td colspan="4"><%= session.Description %></td>
    </tr>
    <tr><td colspan="5">&nbsp;</td></tr>
    <% } %>
    <% } %>
    <% } %>
</table>

<% } %>
