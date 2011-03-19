<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SessionsAddEditModel>" %>
<%@ Import Namespace="EventServer.Core" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Add Session
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
		<script language="javascript" type="text/javascript">
			$(document).ready(function () {
				$("#navSessions").addClass("active");
			});
	</script>

	<%
		if (DateTime.Now > Settings.Instance.SessionSubmissionEndDateTime) 
		{ %>
		<h1>Speaker Submissions Closed!</h1>
		<p>
			The call for speaker sessions closed on <%= Settings.Instance.SessionSubmissionEndDateTime.ToShortDateString() %>.  Thank you for your interest.  
			Please contact <%= Settings.Instance.ContactName %> at <%= Settings.Instance.ContactEmail %> if you have questions or would like to submit a late session.
		</p>
	<%
		} else { %>
		<% using (Html.BeginForm()) {%>

			<%= Html.ValidationSummary(true) %>

			<fieldset>

				<legend>Add Session</legend>

				<div class="editor-label">
					<%= Html.LabelFor(m => m.Title) %>
				</div>
				<div class="editor-field wide">
					<%= Html.TextBoxFor(m => m.Title, new {@class = "focus"}) %>
					<%= Html.ValidationMessageFor(m => m.Title) %>
				</div>

				<div class="editor-label">
					<%= Html.LabelFor(m => m.Category) %>
				</div>
				<div class="editor-field wide">
					<%= Html.DropDownListFor(m => m.Category) %>
					<%= Html.ValidationMessageFor(m => m.Category) %>
				</div>

				<div class="editor-label">
					<%= Html.LabelFor(m => m.Level) %>
				</div>
				<div class="editor-field wide">
					<%= Html.DropDownListFor(m => m.Level) %>
					<%= Html.ValidationMessageFor(m => m.Level) %>
				</div>

				<div class="editor-label">
					<%= Html.LabelFor(m => m.Description) %>
					<%= Html.ValidationMessageFor(m => m.Description) %>
				</div>
				<div class="editor-field">
					<%= Html.TextAreaFor(m => m.Description) %>
				</div>

				<p><input type="submit" value="Add" /></p>

			</fieldset>

			<%= Html.HiddenFor(m => m.Id) %>

		<% } %>
	<% } %>

</asp:Content>
