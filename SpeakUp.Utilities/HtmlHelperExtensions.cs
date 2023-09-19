using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Utilities
{
	public static class HtmlHelperExtensions
	{
		public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "selected")
		{
			string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
			string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

			IEnumerable<string> acceptedActions = (actions ?? "").Split(',').Select(a => a.Trim());
			IEnumerable<string> acceptedControllers = (controllers ?? "").Split(',').Select(c => c.Trim());

			return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : "";
		}
	}
}
