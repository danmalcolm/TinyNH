using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Tests.Core.Domain.NHibernate
{
    public class CategoryPersistenceTests : DatabaseTests
    {
        [Test]
        public void can_persist_and_retrieve_category()
        {
            var original = new Category
            {
                Code = "c-1",
                Name = "Category 1"
            };
            InTransaction(session => session.Save(original));

            Category retrieved = null;
            InTransaction(session => retrieved = session.Get<Category>(original.Id));

            Assert.IsNotNull(retrieved);
            Assert.AreEqual(original.Code, retrieved.Code);
            Assert.AreEqual(original.Name, retrieved.Name);
        }
    }
}