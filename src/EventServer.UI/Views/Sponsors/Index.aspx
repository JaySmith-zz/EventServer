<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SponsorIndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Sponsors
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#navSponsors").addClass("active");
        });
    </script>

    <p>
        We are looking for companies and organizations to contribute to this event. 
        If you are interested, please check out the <%= Html.ActionLink<SponsorsController>(c => c.Sponsorship(), "Become a Sponsor") %> 
        page for levels and contact information.
    </p>
    
    <div align="right">
        <% if (Roles.IsUserInRole("Admin")) { %>
        <%= Html.ActionLink("Add Sponsor", "Create") %>
        <% } %>
    </div>
    
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

     <div class="post">
        <h2 class="title">Past Sponsors</h2>
        <div class="entry">
            <%= Html.ActionLink<SponsorsController>(c => c.PastSponsors(), "View Past Sponsors") %>
        </div>
    </div>
</asp:Content>
