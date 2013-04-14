using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Tests.Core.Domain.NHibernate
{
    public class SupplierPersistenceTests : DatabaseTests
    {
        [Test]
        public void can_persist_and_retrieve_entity()
        {
            var original = new Supplier
            {
                Code = "s-1",
                Name = "Supplier 1"
            };
            InTransaction(session => session.Save(original));

            Supplier retrieved = null;
            InTransaction(session => retrieved = session.Get<Supplier>(original.Id));

            Assert.IsNotNull(retrieved);
            Assert.AreEqual(original.Code, retrieved.Code);
            Assert.AreEqual(original.Name, retrieved.Name);
        }
    }
}