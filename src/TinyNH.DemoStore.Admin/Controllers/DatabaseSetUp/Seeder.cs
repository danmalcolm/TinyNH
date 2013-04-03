using NHibernate;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Admin.Controllers.DatabaseSetUp
{
    public static class Seeder
    {
        public static void SeedDatabase(ISession session)
        {
            var product = new Product
            {
                Code = "product-001",
                Name = "Demo Product 1",
                Description = "Demo product description",
                Supplier = new Supplier
                {
                    Code = "supplier-001",
                    Name = "Demo Supplier 1"
                },
                Category = new Category()
                {
                    Code = "category-001",
                    Name = "Demo Category 1"
                },
            };
            session.Save(product);
        }
    }
}