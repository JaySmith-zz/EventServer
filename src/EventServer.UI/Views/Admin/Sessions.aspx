<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<AdminSessionsModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Sessions
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

        table.sessions {
            width: 100%;
        }

        table.sessions th {
            font-weight: bold;
        }

        table.sessions th, table.sessions td {
            padding: 1px 5px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }

        tr.status_Pending td {
            font-weight: bold;
        }

        tr.hide_pending.status_Pending {
            display: none;
        }
        tr.hide_accepted.status_Accepted {
            display: none;
        }
        tr.hide_rejected.status_Rejected {
            display: none;
        }

        h3.totals_panel {
            float: right;
        }

    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Sessions</h2>

    <p>
        Show:
        <input type="checkbox" id="cbPending" checked="checked" /> Pending
        <input type="checkbox" id="cbAccepted" checked="checked" /> Accepted
        <input type="checkbox" id="cbRejected" checked="checked" /> Rejected
    </p>

    <% foreach (var group in Model.Sessions.GroupBy(x => x.Category)) { %>

        <div style="padding: 15px 0;">

            <h3 class="totals_panel">
                <%= group.Count(x => x.Status == PresentationStatus.Accepted) %> accepted,
                <%= group.Count(x => x.Status == PresentationStatus.Rejected) %> rejected,
                <%= group.Count(x => x.Status == PresentationStatus.Pending) %> pending,
                <%= group.Count() %> total
            </h3>

            <h3><%= group.Key %></h3>

            <table class="sessions" border="0" cellpadding="0" cellspacing="0">

                <col style="width: 65px;" />
                <col style="width: 150px;" />
                <col />
                <col style="width: 90px;" />
                <col style="width: 50px;" />
                <col style="width: 50px;" />

                <thead>
                    <tr>
                        <th>Status</th>
                        <th>Speaker</th>
                        <th>Title</th>
                        <th>Level</th>
                        <th>Track</th>
                        <th>Slot</th>
                    </tr>
                </thead>

                <tbody>
                    <% foreach (var session in group) { %>
                        <tr class="status_<%= session.Status %>">
                            <td><%= session.Status %></td>
                            <td><%= Html.ActionLink<SpeakersController>(c => c.Show(session.UserId, session.SpeakerUrlName), session.SpeakerName) %></td>
                            <td><%= Html.ActionLink<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()), session.Title) %></td>
                            <td><%= session.Level %></td>
                            <td><%= session.Track %></td>
                            <td><%= session.Slot %></td>
                        </tr>
                    <% } %>
                </tbody>

            </table>

        </div>

    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">

    <script type="text/javascript">

        $(function () {

            $('#cbPending').click(function () { $('table.sessions tr').toggleClass('hide_pending'); }).attr('checked', 'checked');
            $('#cbAccepted').click(function () { $('table.sessions tr').toggleClass('hide_accepted'); }).attr('checked', 'checked');
            $('#cbRejected').click(function () { $('table.sessions tr').toggleClass('hide_rejected'); }).attr('checked', 'checked');

        });

    </script>

</asp:Content>
