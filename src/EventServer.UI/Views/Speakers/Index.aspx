<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SpeakersIndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Speakers
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#navSpeakers").addClass("active");
        });
    </script>

    <% for (int i = 0; i < Model.Speakers.Length; i++) { %>

        <%= Html.DisplayFor(m => m.Speakers[i]) %>

    <% } %>

</asp:Content>
