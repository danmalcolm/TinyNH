using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Tests.Integration.Core.Domain.NHibernate
{
    public class ProductPersistenceTests : PersistenceTestsBase
    {
        [Test]
        public void can_persist_and_retrieve_product()
        {
            var original = new Product
            {
                Code = "p-1",
                Name = "Product 1",
                Description = "Product description",
                Supplier = new Supplier
                {
                    Code = "s-1",
                    Name = "Supplier 1"
                },
                Category = new Category()
                {
                    Code = "c-1",
                    Name = "Category 1"
                },
            };
            InTransaction(session => session.Save(original));

            Product retrieved = null;
            InTransaction(session => retrieved = session.Get<Product>(original.Id));

            Assert.IsNotNull(retrieved);
            Assert.AreEqual(original.Code, retrieved.Code);
            Assert.AreEqual(original.Name, retrieved.Name);
            Assert.AreEqual(original.Description, retrieved.Description);
            Assert.AreEqual(original.Supplier.Id, retrieved.Supplier.Id);
            Assert.AreEqual(original.Category.Id, retrieved.Category.Id);
        }
    }
}