namespace TinyNH.DemoStore.Core.Domain
{
	public class Product : Entity
	{
		public virtual string Code { get; set; }

		public virtual string Name { get; set; }

		public virtual string Description { get; set; }

		public virtual int UnitsInStock { get; set; }

		public virtual Supplier Supplier { get; set; }

		public virtual Category Category { get; set; }
	}
}