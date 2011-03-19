<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AccountRegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Welcome!</h2>

    <% using (Html.BeginForm()) { %>

        <%= Html.ValidationSummary(true, "Account creation was unsuccessful. Please correct the errors and try again.") %>

        <div>
            <fieldset>
                <legend>Account Information</legend>

                <div class="editor-label">
                    <%= Html.LabelFor(m => m.Name) %>
                    <%= Html.ValidationMessageFor(m => m.Name)%>
                </div>
                <div class="editor-field">
                    <%= Html.TextBoxFor(m => m.Name, new {@class = "focus"}) %>
                </div>

                <div class="editor-label">
                    <%= Html.LabelFor(m => m.Email) %>
                    <%= Html.ValidationMessageFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBoxFor(m => m.Email) %>
                </div>

                <div class="editor-label">
                    <%= Html.LabelFor(m => m.Password) %>
                    <%= Html.ValidationMessageFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%= Html.PasswordFor(m => m.Password) %>
                </div>

                <div class="editor-label">
                    <%= Html.LabelFor(m => m.ConfirmPassword) %>
                    <%= Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%= Html.PasswordFor(m => m.ConfirmPassword) %>
                </div>

                <p><input type="submit" value="Register" /></p>

            </fieldset>
        </div>

    <% } %>

</asp:Content>
