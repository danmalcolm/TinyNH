using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Admin.Controllers.Products
{
	public class ProductsController : AppController
    {
		public ActionResult Index()
        {
	        var model = new ListModel
	        {
		        Products = CurrentSession.Query<Product>().ToList()
	        };
			return View(model);
        }

        public ActionResult New()
        {
	        var input = new ProductInput();
	        return DisplayEditView(input);
        }

		public ActionResult Edit(Guid id)
		{
			var product = CurrentSession.Get<Product>(id);
			if(product == null)
				return HttpNotFound();

			var input = ProductInput.CreateFrom(product);
			return DisplayEditView(input);
		}

		private ActionResult DisplayEditView(ProductInput input = null)
		{
			var model = new EditModel
			{
				Input = input ?? new ProductInput(),
				CategoryOptions = CurrentSession.Query<Category>()
					.OrderBy(x => x.Name)
					.ToSelectList(x => x.Id.ToString(), x => x.Name),
				SupplierOptions = CurrentSession.Query<Supplier>()
					.OrderBy(x => x.Name)
					.ToSelectList(x => x.Id.ToString(), x => x.Name)
			};
			return View("Edit", model);
		}

        [HttpPost]
        public ActionResult Save(ProductInput input)
        {
			// should probably check for 
			if(!ModelState.IsValid)
			{
				return DisplayEditView(input);
			}

	        Product product;
			if(input.IsNew)
			{
				product = new Product();
			}
			else
			{
				product = CurrentSession.Get<Product>(input.Id);
				if (product == null)
					return HttpNotFound();
			}

	        product.Code = input.Code;
	        product.Name = input.Name;
	        product.Description = input.Description ?? "";
	        product.Supplier = CurrentSession.Get<Supplier>(input.SupplierId);
	        product.Category = CurrentSession.Get<Category>(input.CategoryId);
	        product.UnitsInStock = input.UnitsInStock;
			CurrentSession.SaveOrUpdate(product);

            FlashMessage(string.Format("Product {0} saved", product.Name));

	        return RedirectToAction("Index");
        }
    }
}
