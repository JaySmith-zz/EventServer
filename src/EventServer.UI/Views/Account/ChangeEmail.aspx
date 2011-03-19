<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<AccountChangeEmailModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Change Email
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Change Email</h2>

    <% using (Html.BeginForm()) { %>

        <%= Html.HiddenFor(m => m.Id) %>
        <%= Html.HiddenFor(m => m.CurrentEmail) %>

        <fieldset class="noborder grid">

            <div>
                <div class="display-label"><%= Html.LabelFor(m => m.CurrentEmail) %></div>
                <div class="display-field"><%= Html.DisplayFor(m => m.CurrentEmail) %></div>
            </div>

            <div>
                <div class="editor-label">
                    <%= Html.LabelFor(m => m.NewEmail) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBoxFor(m => m.NewEmail, new {@class = "focus"}) %>
                    <%= Html.ValidationMessageFor(m => m.NewEmail) %>
                </div>
            </div>

            <div class="editor-label">&nbsp;</div>
            <div class="editor-field"><input type="submit" value="Save" /></div>

        </fieldset>

    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
