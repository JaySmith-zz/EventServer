<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<Presentation>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Title %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.DisplayForModel() %>

    <% Html.RenderAction<SessionsController>(c => c.AdminBox(Model.Id)); %>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#navSessions").addClass("active");
        });
    </script>
</asp:Content>
