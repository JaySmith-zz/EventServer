<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EventServer.Core.Domain.Sponsor>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<script language="javascript" type="text/javascript">
		$(document).ready(function () {
			$("#navSponsors").addClass("active");
		});
	</script>

	<h2>Edit</h2>

		<% using (Html.BeginForm()) {%>
		<%= Html.ValidationSummary(true) %>
		
		<fieldset>
			
			<%= Html.HiddenFor(m => m.Id) %>

			<div class="editor-label">
				<%= Html.LabelFor(m => m.IsActive) %>
			</div>
			<div class="editor-field">
				<%= Html.CheckBoxFor(m => m.IsActive)%>
				<%= Html.ValidationMessageFor(m => m.IsActive)%>
			</div>
		   
		   <div class="editor-label">
				<%= Html.LabelFor(m => m.LogoUri) %>
			</div>
			<%= Html.Image(Model.LogoUri) %>
			<div class="editor-field">
				<%= Html.TextBoxFor(m => m.LogoUri)%><br />
				<%= Html.ValidationMessageFor(m => m.LogoUri)%>
			</div>
			
			<div class="editor-label">
				<%= Html.LabelFor(m => m.Name)%>
			</div>
			<div class="editor-field">
				<%= Html.TextBoxFor(m => m.Name)%>
				<%= Html.ValidationMessageFor(m => m.Name)%>
			</div>
			
			<div class="editor-label">
				<%= Html.LabelFor(m => m.Description)%>
			</div>
			<div class="editor-field">
                <%= Html.TextAreaFor(model => model.Description, 20, 20, new { @class = "htmlEditor" })%>
				<%= Html.ValidationMessageFor(m => m.Description)%>
			</div>

			<div class="editor-label">
				<%= Html.LabelFor(m => m.Url)%>
			</div>
			<div class="editor-field">
				<%= Html.TextBoxFor(m => m.Url)%>
				<%= Html.ValidationMessageFor(m => m.Url)%>
			</div>
			
			<div class="editor-label">
				<%= Html.LabelFor(m => m.ContactName)%>
			</div>
			<div class="editor-field">
				<%= Html.TextBoxFor(m => m.ContactName)%>
				<%= Html.ValidationMessageFor(m => m.ContactName) %>
			</div>
			
			<div class="editor-label">
				<%= Html.LabelFor(m => m.ContactEmail) %>
			</div>
			<div class="editor-field">
				<%= Html.TextBoxFor(m => m.ContactEmail) %>
				<%= Html.ValidationMessageFor(n => n.ContactEmail) %>
			</div>

			<div class="editor-label">
				<%= Html.LabelFor(m => m.Level) %>
			</div>
			<div class="editor-field">
				<%= Html.DropDownList("Level" )%>
				<%= Html.ValidationMessageFor(m => m.Level)%>
			</div>
			
			<p>
				<input name="saveButton" type="submit" value="Save" />
				<input name="cancelButton" type="submit" value="Cancel" />
			</p>
		</fieldset>

	<% } %>

	<div>
		<%= Html.ActionLink("Back to List", "Index") %>
	</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="../../Scripts/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            tinymce.init({
                // General options
                mode: "exact",
                elements: "Description",
                theme: "advanced",
                plugins: "inlinepopups,fullscreen,contextmenu,emotions,table,iespell,advlink",
                convert_urls: false,

                // Theme options
                theme_advanced_buttons1: "fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,outdent,indent",
                theme_advanced_buttons2: "iespell,link,unlink,sub,sup,removeformat,cleanup,charmap,emotions,|,formatselect,fontselect,fontsizeselect",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_statusbar_location: "bottom",
                theme_advanced_resizing: true,

                tab_focus: ":prev,:next"
            });

        });
    </script>
</asp:Content>

