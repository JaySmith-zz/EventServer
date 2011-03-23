<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<HomeIndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <% Html.RenderAction<HomeController>(c => c.HomeMessage()); %>
        <% if (Model.Posts.Length > 0)
           { %>
        <h2>
            Recent Announcements <a href="syndication.axd" title="Subscribe to announcements">
                <img src="Content/Images/rss16.png" border="0" /></a></h2>
        <% foreach (var post in Model.Posts)
           { %>
        <div class="post">
            <h1 class="title">
                <%= post.Title %></h1>
            <p class="byline">
                <small>Posted on
                    <%= post.DateCreated.ToLongDateString() %></small></p>
            <div class="entry">
                <%= post.Content %>
            </div>
        </div>
        <% } %>
        <% } %>
    </div>
    <div id="sidebar">
        <% Html.RenderAction<AdminController>(c => c.AdminMenuWidget()); %>
        <% Html.RenderAction<SpeakersController>(c => c.SpeakerMenuWidget()); %>
        <% Html.RenderAction<HomeController>(c => c.Sidebar()); %>
        <div>
            <h2>
                Platinum Sponsors</h2>
            <p align="center">
                <%= Html.ActionLink<SponsorsController>(c => c.Sponsorship(), "Become a Sponsor") %></p>
            <% if (Model.Sponsors.Length > 0)
               {  %>
            <% foreach (var sponsor in Model.Sponsors)
               { %>
            <div>
                <p align="center">
                    <a href="<%= sponsor.Url %>" title="<%= sponsor.Description %>">
                        <img src="<%= sponsor.LogoUri %>" alt="<%= sponsor.Description %>" style="border-style: none;
                            width: 150px" /></a>
                </p>
            </div>
            <% } %>
            <% } %>
        </div>
        <% if (Model.Tweets.Length > 0)
           { %>
        <ul>
            <li>
                <h2>
                    Tweets</h2>
                <% foreach (var tweet in Model.Tweets)
                   { %>
                <div>
                    <p class="byline">
                        <small>
                            <%= tweet.PostDate.ToLongDateString() %></small></p>
                    <div>
                        <%= tweet.Message %>
                    </div>
                </div>
                <% } %>
            </li>
        </ul>
        <% } %>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script language="javascript" type="text/javascript">

        $(function () {
            $("#navHome").addClass("active");
        });

    </script>
</asp:Content>