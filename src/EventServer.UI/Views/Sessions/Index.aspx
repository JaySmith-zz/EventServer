<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SessionsIndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Sessions
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">

        .tab-group {
            margin-bottom: 15px;
            border-bottom: 1px solid #777;
            height: 30px;
        }

        .tabs {
            float: right;
        }

        .tabs a {
            display: block;
            float: left;
            padding: 5px 15px;
            margin: 0 10px;
            text-decoration: none;
            color: inherit;
            border: solid 1px #fff;
        }

        .tabs a:hover, .tabs a.you-are-here {
            border-style: solid;
            border-width: 1px;
            border-color: #777 #777 #fff #777;
            padding-bottom: 6px;
        }

        .tabs a.you-are-here {
            color: #000;
        }

    </style>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <div class="tab-group">
        <div class="tabs">
            <%= Html.ActionLink<SessionsController>(c => c.Index("list"), "list", new { @class = Model.ListLinkCssClass }) %>
            <%= Html.ActionLink<SessionsController>(c => c.Index("tracks"), "tracks", new { @class = Model.TracksLinkCssClass }) %>
            <%= Html.ActionLink<SessionsController>(c => c.Index("times"), "times", new { @class = Model.TimesLinkCssClass }) %>
        </div>
    </div>

    <div id="sessions-container">

        <% Html.RenderAction(Model.ContentAction); %>

    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">

    <script language="javascript" type="text/javascript">
        $(function () {
            $("#navSessions").addClass("active");
        });
    </script>

</asp:Content>
