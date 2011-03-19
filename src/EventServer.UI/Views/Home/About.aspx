<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    About
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#navAbout").addClass("active");
        });
    </script>

    <% Html.RenderAction<HomeController>(c => c.AboutMessage()); %>

</asp:Content>
