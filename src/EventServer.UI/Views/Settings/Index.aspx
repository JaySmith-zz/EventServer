<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AppSettings>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Settings</h2>
    <% using (Html.BeginForm())
       {%>
    <%= Html.ValidationSummary(true) %>
    <fieldset>
        <legend>Site</legend>

        <div class="editor-label">
            <%= Html.LabelFor(model => model.SiteName)%>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.SiteName) %>
            <%= Html.ValidationMessageFor(model => model.SiteName) %>
        </div>
        
        <div class="editor-label">
            <%= Html.LabelFor(model => model.SiteSlogan) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.SiteSlogan) %>
            <%= Html.ValidationMessageFor(model => model.SiteSlogan) %>
        </div>

        <div class="editor-label">
            <%= Html.LabelFor(model => model.SiteTheme) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.SiteTheme)%>
            <%= Html.ValidationMessageFor(model => model.SiteTheme)%>
        </div>

        <div class="editor-label">
            <%= Html.LabelFor(model => model.Description) %>
        </div>
        <div class="editor-field">
            <%= Html.TextAreaFor(model => model.Description, 10, 60, new { @rows = "10", @cols = "60" }) %>
            <%= Html.ValidationMessageFor(model => model.Description) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.StartDateTime) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.StartDateTime, String.Format("{0:g}", Model.StartDateTime)) %>
            <%= Html.ValidationMessageFor(model => model.StartDateTime) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.EndDateTime) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.EndDateTime, String.Format("{0:g}", Model.EndDateTime)) %>
            <%= Html.ValidationMessageFor(model => model.EndDateTime) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.RegistrationEndDateTime) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.RegistrationEndDateTime, String.Format("{0:g}", Model.RegistrationEndDateTime)) %>
            <%= Html.ValidationMessageFor(model => model.RegistrationEndDateTime) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.SessionSubmissionEndDateTime) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.SessionSubmissionEndDateTime, String.Format("{0:g}", Model.SessionSubmissionEndDateTime)) %>
            <%= Html.ValidationMessageFor(model => model.SessionSubmissionEndDateTime) %>
        </div>
    </fieldset>
    <fieldset>
        <legend>Venue</legend>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.VenueName) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.VenueName) %>
            <%= Html.ValidationMessageFor(model => model.VenueName) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.VenuePhone) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.VenuePhone) %>
            <%= Html.ValidationMessageFor(model => model.VenuePhone) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.VenueStreet) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.VenueStreet) %>
            <%= Html.ValidationMessageFor(model => model.VenueStreet) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.VenueCity) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.VenueCity) %>
            <%= Html.ValidationMessageFor(model => model.VenueCity) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.VenueState) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.VenueState) %>
            <%= Html.ValidationMessageFor(model => model.VenueState) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.VenueZip) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.VenueZip) %>
            <%= Html.ValidationMessageFor(model => model.VenueZip) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.ContactName) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.ContactName) %>
            <%= Html.ValidationMessageFor(model => model.ContactName) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.ContactEmail) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.ContactEmail) %>
            <%= Html.ValidationMessageFor(model => model.ContactEmail) %>
        </div>
    </fieldset>
    <fieldset>
        <legend>Twitter</legend>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.TwitterId) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.TwitterId) %>
            <%= Html.ValidationMessageFor(model => model.TwitterId) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.SiteLogoUri) %>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.SiteLogoUri) %>
            <%= Html.ValidationMessageFor(model => model.SiteLogoUri) %>
        </div>
        <p>
            <input type="submit" value="Save" />
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
</asp:Content>
