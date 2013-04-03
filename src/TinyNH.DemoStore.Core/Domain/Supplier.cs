namespace TinyNH.DemoStore.Core.Domain
{
	public class Supplier : Entity
	{
		public virtual string Code { get; set; }

		public virtual string Name { get; set; }
	}
}