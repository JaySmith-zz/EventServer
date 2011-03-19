<%@ Control Language="C#" Inherits="ViewUserControl<UserProfile>" %>
<div class="post">
    <h2 class="title">
        <%=Html.ActionLink<SpeakersController>(c => c.Show(Model.Id, Model.UrlName), Model.Name)%></h2>
    <div class="entry">
        <table border="0" cellpadding="3" cellspacing="0">
            <tr>
                <td valign="top">
                    <% string imageUrl = Model.SpeakerProfile.HasImageUrl ? Model.SpeakerProfile.ImageUrl : "~/Content/Images/PhotoNotAvailable.gif"; %>
                    <% if (Model.SpeakerProfile.HasBlogUrl) { %>
                        <p><a href="<%= Model.SpeakerProfile.BlogUrl %>">
                            <%=Html.Image(imageUrl, "Speaker Image", new { width = 100, border = 0 })%></a>
                        </p>
                    <% } else { %>
                        <p><%=Html.Image(imageUrl, "Speaker Image", new { width = 100, border = 0 })%></p>
                    <% } %>

                    <% if (Model.SpeakerProfile.IsMvp) { %>
                        <p><a href="<%= Model.SpeakerProfile.MvpProfileUrl %>">Microsoft MVP</a></p>
                    <% } %>

                    <% if (string.IsNullOrEmpty(Model.SpeakerProfile.BlogUrl)) { %>
                        <p><a href="<%= Model.SpeakerProfile.BlogUrl %>">Blog</a></p>
                    <% } %>
                </td>
                <td>
                    <%= Html.ToParagraphs(Model.SpeakerProfile.Biography) %>
                </td>
            </tr>
        </table>
    </div>
</div>
