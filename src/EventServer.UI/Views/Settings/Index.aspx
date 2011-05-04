<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AppSettings>" %>

<%@ Import Namespace="System.Web.UI.MobileControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Event Server Settings
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Event Server Settings</h2>
    <br />

    <%= Html.ActionLink<SettingsController>(c => c.Twitter(), "Twitter") %>

    <% using (Html.BeginForm())
       {%>
    <%= Html.ValidationSummary(true) %>
    <h3>
        General Settings</h3>
    <fieldset>
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
            <%= Html.DropDownListFor(model => model.SiteTheme, (List<SelectListItem>) ViewData["AvailableThemes"])%>
            <%= Html.ValidationMessageFor(model => model.AvailableThemes)%>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(model => model.Description) %>
        </div>
        <div class="editor-field">
            <%= Html.TextAreaFor(model => model.Description) %>
            <%= Html.ValidationMessageFor(model => model.Description) %>
        </div>

        <div class="editor-label">
            <%= Html.LabelFor(model => model.NumberOfDaysForEvent) %>
        </div>
        <div class="editor-field">
            <%= Html.DropDownListFor(model => model.NumberOfDaysForEvent, (List<SelectListItem>)ViewData["NumberOfDays"])%>
            <%= Html.ValidationMessageFor(model => model.NumberOfDaysForEvent)%>
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
    <h3>
        Event Location</h3>
    <fieldset>
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
    <p>
        <input style="align: right;" type="submit" value="Save" />
    </p>
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
