<%@ Control Language="C#" Inherits="ViewUserControl<SessionsAdminBoxModel>" %>

<div style="padding: 20px; overflow: hidden;">

    <% if (Model.CanEdit) { %>

        <div style="float: left;">
            <%= Html.ActionLink<SessionsController>(c => c.Edit(Model.SessionId), "edit") %>
            <%= Html.ActionLink<SessionsController>(c => c.Delete(Model.SessionId), "delete") %>
        </div>

    <% } %>

    <% if (Model.CanAcceptReject) { %>

        <div style="float: right;">

            <%= Html.ActionLink<SessionsController>(c => c.Accept(Model.SessionId), "accept") %>
            <%= Html.ActionLink<SessionsController>(c => c.Reject(Model.SessionId), "reject") %>

            &bull;

            <%= Html.ActionLink<AdminController>(c => c.Sessions(), "Sessions List") %>

        </div>

    <% } %>

    <h2 style="clear: both;">Comments</h2>

    <div id="comments">

        <% for (int i = 0; i < Model.Comments.Length; i++) { %>

            <%= Html.DisplayFor(m => m.Comments[i]) %>

        <% } %>

    </div>

    <% using (Ajax.BeginForm(
           "PostComment",
           "Sessions",
           new AjaxOptions {HttpMethod="Post", UpdateTargetId = "comments", InsertionMode = InsertionMode.InsertAfter, OnBegin = "sessionAdminBox.beginPost", OnSuccess = "sessionAdminBox.commentPosted"},
           new {id = "postComment"})) { %>

        <%= Html.Hidden("id", Model.SessionId) %>
        <%= Html.Hidden("author", Model.User.Name) %>

        <fieldset>
            <legend>Add Comment</legend>
            <div class="editor-field"><%= Html.TextArea("Content", new {@class = "required"})%></div>
            <p><input type="submit" value="Post Comment" /></p>
        </fieldset>

    <% }%>

</div>
