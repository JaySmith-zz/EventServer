using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using Microsoft.Web.Mvc;

public static class MvcExtensionMethods
{
    public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> selector)
    {
        var values = typeof(TProperty).IsEnum
                         ? Enum.GetNames(typeof(TProperty))
                         : typeof(TProperty).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.GetValue(null).ToString());

        return helper.DropDownListFor(selector, values.Select(x => new SelectListItem { Text = x, Value = x }));
    }

    public static MvcHtmlString ToParagraphs<TModel>(this HtmlHelper<TModel> helper, string source)
    {
        var parsed = helper
            .Encode(source)
            .Replace("\r", string.Empty)
            .Replace("\n\n", "</p><p>")
            .Replace("\n", "<br />");

        return MvcHtmlString.Create("<p>{0}</p>".F(parsed));
    }

    public static string Link<TModel>(this HtmlHelper<TModel> helper, string relation, string title, string href)
    {
        const string Output = @"<link type=""{0}"" rel=""edituri"" title=""{1}"" href=""http://{2}"" />";

        var parsed = Output
            .Replace("{0}", relation)
            .Replace("{1}", title)
            .Replace("{2}", href);

        return parsed;
    }

    public static string Url<TController>(this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller
    {
        return SystemPath.BaseUri() + helper.BuildUrlFromExpression(action);
    }

    public static MvcHtmlString FullActionLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller
    {
        return helper.FullActionLink(action, helper.Url(action));
    }

    public static MvcHtmlString FullActionLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string linkText) where TController : Controller
    {
        return helper.FullActionLink(action, linkText, null);
    }

    public static MvcHtmlString FullActionLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string linkText, object htmlAttributes) where TController : Controller
    {
        var builder = new TagBuilder("a")
            { InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty };

        builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        builder.MergeAttribute("href", helper.Url(action));

        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }

    public static bool IsCurrentUserOwnerOrAdmin(this HtmlHelper helper, Session session)
    {
        if (session == null)
        {
            return false;
        }

        var currentUser = DependencyResolver.Current.GetService<ICurrentUserService>();

        return currentUser.IsAdmin || currentUser.Owns(session);
    }
}