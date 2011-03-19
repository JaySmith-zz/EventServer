<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<AccountShowModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.User.Name %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <fieldset class="noborder grid">

        <div>
            <div class="display-label">Name:</div>
            <div class="display-field"><%= Html.ActionLink<AccountController>(c => c.ChangeName(Model.User.Id), Model.User.Name) %></div>
        </div>

        <div>
            <div class="display-label">Email:</div>
            <div class="display-field"><%= Html.ActionLink<AccountController>(c => c.ChangeEmail(Model.User.Id), Model.User.Email) %></div>
        </div>

        <div>
            <div class="display-label">Password:</div>
            <div class="display-field"><%= Html.ActionLink<AccountController>(c => c.ChangePassword(Model.User.Id), "change") %></div>
        </div>

        <div>
            <div class="display-label">Speaker Profile:</div>
            <div class="display-field">
                <% if (Model.User.SpeakerProfile == null) { %>
                    <%= Html.ActionLink<SpeakersController>(c => c.Create(Model.User.Id), "create") %>
                <% } else { %>
                    <%= Html.ActionLink<SpeakersController>(c => c.Edit(Model.User.Id), "edit") %>
                    <%= Html.ActionLink<SpeakersController>(c => c.Show(Model.User.Id, Model.User.UrlName), "view") %>
                <% } %>
            </div>
        </div>

    </fieldset>

</asp:Content>
