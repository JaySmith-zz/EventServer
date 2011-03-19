<%@ Control Language="C#" Inherits="ViewUserControl<Comment>" %>

<div class="post">

    <div class="entry">
        <%= Html.ToParagraphs(Model.Content) %>
    </div>

    <p class="byline" style="font-size: .8em;">
        Posted on <%= Model.DateCreated.ToLongDateString() %><br />
        by <%= Html.Encode(Model.Author) %>
    </p>

</div>
