<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EventServer.Core.Domain.Sponsor>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

        <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>
             
             <%= Html.HiddenFor(m => m.Id) %>

            <div class="editor-label">
                <%= Html.LabelFor(model => model.IsActive) %>
            </div>
            <div class="editor-field">
                <%= Html.CheckBoxFor(model => model.IsActive) %>
                <%= Html.ValidationMessageFor(model => model.IsActive) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.LogoUri)%>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.LogoUri) %>
                <%= Html.ValidationMessageFor(model => model.LogoUri) %>
            </div>

            <div class="editor-label">
                <%= Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Description) %>
            </div>
            <div class="editor-field">
                <%= Html.TextAreaFor(model => model.Description, 20, 20, new { @class = "htmlEditor" })%>
                <%= Html.ValidationMessageFor(model => model.Description) %>
            </div>

            <div class="editor-label">
                <%= Html.LabelFor(model => model.Url) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Url)%>
                <%= Html.ValidationMessageFor(model => model.Url)%>
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

            <div class="editor-label">
                <%= Html.LabelFor(model => model.Level) %>
            </div>
            <div class="editor-field">
                <%= Html.DropDownList("Level" )%>
                <%= Html.ValidationMessageFor(model => model.Level)%>
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
    <script src="../Scripts/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#navSponsors").addClass("active");

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

