<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserProfile>" %>

<% if (!Request.IsAuthenticated) { %>

    [ <%= Html.ActionLink<AccountController>(c => c.LogOn(""), "Log On") %> ]

<% } else { %>

    <a href="<%= Html.BuildUrlFromExpression<AccountController>(c => c.Show(Model.Id, Model.UrlName)) %>">Welcome <b><%= Html.Encode(Model.Name) %></b>!</a>

    [ <%= Html.ActionLink<AccountController>(c => c.LogOff(), "Log Off") %> ]

<% } %>
