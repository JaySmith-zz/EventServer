<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<AccountChangeNameModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Change Name
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Change Name</h2>

    <% using (Html.BeginForm()) { %>

        <%= Html.HiddenFor(m => m.Id) %>

        <fieldset>

            <div class="editor-label">
                <%= Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Name, new {@class = "focus"}) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </div>

            <p><input type="submit" value="Save" /></p>

        </fieldset>

    <% } %>

</asp:Content>
