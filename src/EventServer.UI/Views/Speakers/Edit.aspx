<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SpeakersEditModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Edit
    <%= Model.Name %>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#navSpeakers").addClass("active");
        });
    </script>

    <% using (Html.BeginForm())
       { %>
    <%= Html.HiddenFor(m => m.SpeakerId) %>
    <%= Html.HiddenFor(m => m.Name) %>
    <%= Html.HiddenFor(m => m.Email) %>
    <fieldset>
        <legend>Speaker Information</legend>
        <%= Html.ValidationSummary(true) %>
        <div class="editor-label">
            <%= Html.LabelFor(m => m.Bio) %>
            <%= Html.ValidationMessageFor(m => m.Bio) %>
        </div>
        <div class="editor-field">
            <%= Html.TextAreaFor(m => m.Bio) %>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(m => m.ImageUrl)%>
            <%= Html.ValidationMessageFor(m => m.ImageUrl)%>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(m => m.ImageUrl)%>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(m => m.BlogUrl)%>
            <%= Html.ValidationMessageFor(m => m.BlogUrl)%>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(m => m.BlogUrl)%>
        </div>
        <div class="editor-field">
            <%= Html.CheckBoxFor(m => m.IsMvp)%>
            <%= Html.LabelFor(m => m.IsMvp)%>
        </div>
        <div class="editor-label">
            <%= Html.LabelFor(m => m.MvpProfileUrl)%>
            <%= Html.ValidationMessageFor(m => m.MvpProfileUrl)%>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(m => m.MvpProfileUrl)%>
        </div>
        <hr />
        <div class="editor-field">
            <%= Html.CheckBoxFor(m => m.TravelAssistance) %>
            <%= Html.LabelFor(m => m.TravelAssistance) %>
        </div>
        <p>
            <input type="submit" value="Save" /></p>
    </fieldset>
    <% } %>
</asp:Content>
