using System;
using System.Reflection;
using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Tests.Core.Domain.NHibernate
{
    public class EntityProxyDetectionTests : DatabaseTests
    {
        [Test]
        public void GetTypeUnproxied_should_reveal_type_being_proxied()
        {
            var supplier1 = new Supplier
            {
                Code = "s-1",
                Name = "Supplier 1"
            };
            InTransaction(session => session.Save(supplier1));

            var getTypeUnproxiedMethod = typeof (Supplier).GetMethod("GetTypeUnproxied", BindingFlags.NonPublic | BindingFlags.Instance);
            InTransaction(session =>
            {
                var supplier2 = session.Load<Supplier>(supplier1.Id);
                var type = supplier2.GetType();

                StringAssert.Contains("Proxy", type.Name);
                Type typeUnproxied = (Type)getTypeUnproxiedMethod.Invoke(supplier2, null);
                Assert.AreEqual(typeof(Supplier), typeUnproxied);
            });
        }
    }
}