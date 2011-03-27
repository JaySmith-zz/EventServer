<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Session[]>" %>

<% for (int i = 0; i < Model.Length; i++) { %>

    <%= Html.DisplayFor(m => m[i]) %>

<% } %>
