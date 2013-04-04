using System;
using System.Reflection;
using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.Tests.Integration.Core.Domain.NHibernate
{
	[TestFixture]
	public class EntityTests
	{
		private static readonly PropertyInfo IdProperty = typeof (Entity).GetProperty("Id");
		private void AssignId(Entity entity, Guid id)
		{
			// Brute force assigning of Id to simulate persisted entity
			IdProperty.SetValue(entity, id);
		}

		public class TestEntity1 : Entity { }

		public class TestEntity1Proxy : TestEntity1 { }

		public class TestEntity2 : Entity { }

		[Test]
		public void persisted_entity_should_be_equal_to_null()
		{
			var entity = new TestEntity1();

			Assert.IsFalse(Equals(entity, null));
		}

		[Test]
		public void persisted_entity_should_be_equal_to_self()
		{
			var entity = new TestEntity1();
			AssignId(entity, Guid.NewGuid());

			Assert.IsTrue(Equals(entity, entity));
		}

		[Test]
		public void transient_entity_should_be_equal_to_self()
		{
			var entity = new TestEntity1();

			Assert.IsTrue(Equals(entity,entity));
		}
		
		[Test]
		public void persisted_entity_should_be_equal_to_another_instance_with_matching_id()
		{
			var entity1 = new TestEntity1();
			var entity2 = new TestEntity1();
			AssignId(entity1, Guid.NewGuid());
			AssignId(entity2, entity1.Id);

			Assert.IsTrue(entity1.Equals(entity2));
		}

		[Test]
		public void persisted_entity_should_be_equal_to_another_instance_of_derived_type_with_matching_id()
		{
			var entity1 = new TestEntity1();
			var entity2 = new TestEntity1Proxy();
			AssignId(entity1, Guid.NewGuid());
			AssignId(entity2, entity1.Id);

			Assert.IsTrue(entity1.Equals(entity2));
		}

		[Test]
		public void persisted_entity_should_not_be_equal_to_another_instance_of_different_type_with_matching_id()
		{
			var entity1 = new TestEntity1();
			var entity2 = new TestEntity2();
			AssignId(entity1, Guid.NewGuid());
			AssignId(entity2, entity1.Id);

			Assert.IsFalse(entity1.Equals(entity2));
		}

		[Test]
		public void transient_entity_should_not_be_equal_to_another_transient_instance()
		{
			var entity1 = new TestEntity1();
			var entity2 = new TestEntity2();

			Assert.IsFalse(Equals(entity1, entity2));
		}

	}
}
