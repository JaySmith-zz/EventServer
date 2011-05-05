<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EventServer.Core.ViewModels.SettingsTwitterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Twitter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Twitter</h2>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>
        
        <fieldset>
          
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Id) %>
                <%= Html.TextBoxFor(model => model.Id) %>
                <%= Html.ValidationMessageFor(model => model.Id) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.FilterDate) %>
                           <%= Html.TextBoxFor(model => model.FilterDate, String.Format("{0:g}", Model.FilterDate)) %>
                <%= Html.ValidationMessageFor(model => model.FilterDate) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.DisplayCount) %>
                <%= Html.TextBoxFor(model => model.DisplayCount) %>
                <%= Html.ValidationMessageFor(model => model.DisplayCount) %>
            </div>
            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%= Html.ActionLink("Back to Admin", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

