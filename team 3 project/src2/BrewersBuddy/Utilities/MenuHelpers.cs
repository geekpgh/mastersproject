using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BrewersBuddy.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Slightly modified from http://stackoverflow.com/questions/5084635/custom-actionlink-helper-that-knows-what-page-youre-on
    /// </remarks>
    public static class MenuHelpers
    {
        public static MvcHtmlString MenuActionLink(
            this HtmlHelper htmlHelper,
            string linkText,
            string action,
            string controller
        )
        {
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var listItem = new TagBuilder("li");

            if (controller.Equals(currentController))
                listItem.AddCssClass("active");

            listItem.InnerHtml = htmlHelper.ActionLink(linkText, action, controller)
                .ToHtmlString();

            return MvcHtmlString.Create(listItem.ToString());
        }

        public static MvcHtmlString SubmenuActionLink(
            this HtmlHelper htmlHelper,
            string linkText,
            string action,
            string controller
        )
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var listItem = new TagBuilder("li");

            if (controller.Equals(currentController) && action.Equals(currentAction))
                listItem.AddCssClass("active");

            listItem.InnerHtml = htmlHelper.ActionLink(linkText, action, controller)
                .ToHtmlString();

            return MvcHtmlString.Create(listItem.ToString());
        }
    }
}