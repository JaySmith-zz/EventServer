<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EventServer.Core.Domain.UserProfile>" %>

<ul>
    <li>
        <h2><%= Html.Encode(Model.Name) %></h2>
        <div style="padding: 10px;">

            <% if (Model.IsSpeaker) { %>

                <%= Html.ActionLink<SpeakersController>(c => c.Show(Model.Id, Model.UrlName), "Your Profile")%> (<%= Html.ActionLink<SpeakersController>(c => c.Edit(Model.Id), "edit") %>)<br />
                <%= Html.ActionLink<SessionsController>(c => c.Add(), "Add New Session") %><br />

            <% } else { %>

                <%= Html.ActionLink<SpeakersController>(c => c.Create(Model.Id), "Create Speaker Profile") %><br />

            <% } %>

            <%= Html.ActionLink<AccountController>(c => c.ChangeEmail(Model.Id), "Change Your Email") %><br />
            <%= Html.ActionLink<AccountController>(c => c.ChangePassword(Model.Id), "Change Your Password") %><br />

        </div>
    </li>
</ul>
