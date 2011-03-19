<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<AdminUsersModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Users
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

        ul.users {
            padding: 0;
        }

        sup {
            font-style: normal;
            font-weight: normal;
            line-height: 100%;
        }

        ol {
            line-height: 100%;
            margin-top: 2px;
            margin-bottom: 2em;
        }

    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Users</h2>

    <ul class="users">

        <% for (int i = 0; i < Model.Users.Length; i++) { %>

            <li>
                <%= Html.HiddenFor(m => Model.Users[i].User.Email, new {@class = "user-email"}) %>
                <%= Html.CheckBoxFor(m => Model.Users[i].IsAdmin, new {title = "Administrator", @class = "isAdmin"}) %>
                <%= Html.ActionLink<AccountController>(c => c.Show(Model.Users[i].User.Id, Model.Users[i].User.UrlName), "edit") %>
                <%= Html.ActionLink<AccountController>(c => c.Delete(Model.Users[i].User.Id), "delete") %>
                <%= Html.Mailto(Model.Users[i].User.Email, Model.Users[i].User.Email) %>
                <%= Model.Users[i].User.Name %>
                <% if (Model.Users[i].RequestingTravelAssistance) { %>
                    <sup title="requesting travel assistance"><a href="#note-1">[1]</a></sup>
                <% } %>
            </li>

        <% } %>

    </ul>

    Notes:
    <ol>
        <li id="note-1">User is requesting travel assistance</li>
    </ol>


    <% using (Html.BeginForm()) { %>

        <fieldset class="grid">
            <legend>New user</legend>

            <div class="editor-label"><%= Html.LabelFor(m => m.NewUser.Name) %></div>
            <div class="editor-field"><%= Html.TextBoxFor(m => m.NewUser.Name, new {@class = "focus"}) %></div>

            <div class="editor-label"><%= Html.LabelFor(m => m.NewUser.Email) %></div>
            <div class="editor-field"><%= Html.TextBoxFor(m => m.NewUser.Email) %></div>

            <div class="editor-label"><%= Html.LabelFor(m => m.NewUser.Password) %></div>
            <div class="editor-field"><%= Html.PasswordFor(m => m.NewUser.Password) %></div>

            <div class="editor-label"><%= Html.LabelFor(m => m.NewUser.ConfirmPassword) %></div>
            <div class="editor-field"><%= Html.PasswordFor(m => m.NewUser.ConfirmPassword) %></div>

            <div class="editor-label">&nbsp;</div>
            <div class="editor-field"><input type="submit" value="Create" /></div>
        </fieldset>

        <%= Html.ValidationSummary() %>

    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">

    <script type="text/javascript">

        $(function() {

            $('.isAdmin').click(function() {

                var checkbox = $(this);
                var email = checkbox.parents('li:first').find(':hidden.user-email').val();

                $.post(
                    webRoot + 'Admin/AlterAdmin',
                    { email: email, isAdmin: checkbox.is(':checked') },
                    function(result) {

                        $.growl('Administrator', result.Message, result.Success ? null : $.growl.settings.errorImage);

                        if (!result.Success) {
                            checkbox.attr('checked', !checkbox.is(':checked'));
                        }

                    });

            });

        });

    </script>
</asp:Content>