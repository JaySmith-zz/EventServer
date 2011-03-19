<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SessionsAddEditModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Edit session
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) {%>

        <%= Html.ValidationSummary(true) %>

        <fieldset>

            <legend>Edit Session</legend>

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
            </div>
            <div class="editor-field">
                <%= Html.TextAreaFor(m => m.Description) %>
                <%= Html.ValidationMessageFor(m => m.Description) %>
            </div>

            <% if (Roles.IsUserInRole("Admin")) {%>

            <div class="editor-label">
                <%= Html.LabelFor(m => m.Track) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(m => m.Track) %>
                <%= Html.ValidationMessageFor(m => m.Track) %>
            </div>

            <div class="editor-label">
                <%= Html.LabelFor(m => m.TimeSlot) %>
            </div>
            <div class="editor-field">
                <%= Html.DropDownList("TimeSlot") %>
                <%= Html.ValidationMessageFor(m => m.TimeSlot) %>
            </div>

            <% } %>

            <p><input type="submit" value="Save" /></p>

        </fieldset>

        <%= Html.HiddenFor(m => m.Id) %>

    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#navSessions").addClass("active");
        });
    </script>
</asp:Content>
