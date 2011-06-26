<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SponsorIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dallas TechFest 2010 : Sponsors
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#navSponsors").addClass("active");
        });
    </script>

    <p>
        We are very thankful for companies and organizations that have contributed to the Dallas TechFest in the past. 
        If you are interested in being involved in Dallas TechFest, please check out the <%= Html.ActionLink<SponsorsController>(c => c.Sponsorship(), "Become a Sponsor") %> 
        page for levels and contact information.
    </p>
       
    <div class="post">
        <h2 class="title">
            Platinum Sponsors
        </h2>
        <div class="entry">
            <% for (int i = 0; i < Model.PlatinumSponsors.Length; i++) { %>
            <%= Html.DisplayFor(m => m.PlatinumSponsors[i])%>
            <% } %>
        </div>
    </div>
    
    <div class="post">
        <h2 class="title">
            Gold Sponsors</h2>
        <div class="entry">
            <% for (int i = 0; i < Model.GoldSponsors.Length; i++) { %>
            <%= Html.DisplayFor(m => m.GoldSponsors[i])%>
            <% } %>
        </div>
    </div>

    <% if (Roles.IsUserInRole("Admin")) { %>
    <div class="post">
        <h2 class="title">
            Inactive Sponsors</h2>
        <div class="entry">
            <% for (int i = 0; i < Model.InactiveSponsors.Length; i++) { %>
            <%= Html.DisplayFor(m => m.InactiveSponsors[i])%>
            <% } %>
        </div>
    </div>
    <% } %>

</asp:Content>
