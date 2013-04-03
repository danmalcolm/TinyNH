using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TinyNH.DemoStore.Admin.Controllers
{
	public static class ControllerExtensions
	{
		 public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> sequence, Func<T,string> getValue, Func<T,string> getText)
		 {
			 var items = sequence.Select(x => new SelectListItem {Value = getValue(x), Text = getText(x)});
			 return items.ToList();
		 }
	}
}