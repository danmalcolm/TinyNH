namespace TinyNH.DemoStore.Core.Domain
{
	public class Category : Entity
	{
		public virtual string Code { get; set; }

		public virtual string Name { get; set; }
	}
}