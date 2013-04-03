using System.Collections.Generic;
using System.Web.Mvc;

namespace TinyNH.DemoStore.Admin.Controllers.Products
{
	public class EditModel
	{
		public ProductInput Input { get; set; }

		public List<SelectListItem> SupplierOptions { get; set; } 

		public List<SelectListItem> CategoryOptions { get; set; } 
	}
}