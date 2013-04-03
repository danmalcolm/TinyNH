using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NHibernate.Linq;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Admin.Controllers.Products
{
	/// <summary>
	/// A version of the product entity used to display and to bind to changes made in form
	/// </summary>
	public class ProductInput : IValidatableObject
	{
		public static ProductInput CreateFrom(Product product)
		{
			return new ProductInput
			{
				Id = product.Id,
				Code = product.Code,
				Name = product.Name,
				Description = product.Description,
				SupplierId = product.Supplier.Id,
				CategoryId = product.Category.Id
			};
		}

		public ProductInput()
		{
			Id = Guid.Empty;
			Code = "";
			Name = "";
			Description = "";
			SupplierId = Guid.Empty;
			CategoryId = Guid.Empty;
		}

		public Guid Id { get; set; }

		[Required, MaxLength(20)]
		public string Code { get; set; }

		[Required, MaxLength(50)]
		public string Name { get; set; }

		[MaxLength(1000)]
		public string Description { get; set; }

		[Required]
		public Guid SupplierId { get; set; }

		[Required]
		public Guid CategoryId { get; set; }

		[Range(0,1000)]
		public int UnitsInStock { get; set; }

		public bool IsNew
		{
			get { return Id == Guid.Empty; }
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var query = MvcApplication.CurrentSession.Query<Product>().Where(x => x.Code == Code);
			if(!IsNew)
			{
				query = query.Where(x => x.Id != Id);
			}
			bool otherEntityUsingCode = query.Any();
			var results = new List<ValidationResult>();
			if(otherEntityUsingCode)
			{
				results.Add(new ValidationResult("The code is already in use", new[] { "Code" }));
			}
			return results;
		}
	}
}