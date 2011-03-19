<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul>
    <li>
        <h2>Administrators</h2>
        <div style="padding: 10px;">
            <%= Html.ActionLink<SettingsController>(c => c.Index(), "Settings") %><br />
            <%= Html.ActionLink<AdminController>(c => c.Users(), "Users") %><br />
            <%= Html.ActionLink<AdminController>(c => c.Sessions(), "Sessions") %><br />
        </div>
    </li>
</ul>
