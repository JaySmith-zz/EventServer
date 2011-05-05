<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EventServer.Core.ViewModels.EmailSettingsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Email Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Email Settings</h2>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>
        
        <fieldset>

             <div class="editor-label">
                <%= Html.LabelFor(model => model.EmailEnabled) %>
                <%= Html.CheckBoxFor(model => model.EmailEnabled)%>
                <%= Html.ValidationMessageFor(model => model.EmailEnabled)%>
            </div>

            <div class="editor-label">
                <%= Html.LabelFor(model => model.EmailFromAddress) %>
                <%= Html.TextBoxFor(model => model.EmailFromAddress) %>
                <%= Html.ValidationMessageFor(model => model.EmailFromAddress) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.EmailHost) %>
                <%= Html.TextBoxFor(model => model.EmailHost) %>
                <%= Html.ValidationMessageFor(model => model.EmailHost) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.EmailHostPort) %>
                <%= Html.TextBoxFor(model => model.EmailHostPort) %>
                <%= Html.ValidationMessageFor(model => model.EmailHostPort) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.UserName) %>
                <%= Html.TextBoxFor(model => model.UserName) %>
                <%= Html.ValidationMessageFor(model => model.UserName) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Password) %>
                <%= Html.TextBoxFor(model => model.Password) %>
                <%= Html.ValidationMessageFor(model => model.Password) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.EnableSsl) %>
                <%= Html.CheckBoxFor(model => model.EnableSsl) %>
                <%= Html.ValidationMessageFor(model => model.EnableSsl) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.SubjectPrefix) %>
                <%= Html.TextBoxFor(model => model.SubjectPrefix) %>
                <%= Html.ValidationMessageFor(model => model.SubjectPrefix) %>
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

