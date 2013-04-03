using System;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace TinyNH.DemoStore.Core.Domain.NHibernate
{
    /// <summary>
    /// Adds model mapping to NHibernate Configuration
    /// </summary>
	public static class ModelMapping
	{
		public static void Add(Configuration configuration)
		{
			var mapper = new ModelMapper();

			mapper.BeforeMapClass += MapClass;
			mapper.BeforeMapProperty += MapProperty;
			mapper.BeforeMapManyToOne += MapManyToOne;

			mapper.Class<Product>(productClass =>
			{
				productClass.Property(x => x.Code, prop =>
				{
					prop.Length(20);
					prop.Unique(true);
				});
				productClass.Property(x => x.Name, prop => prop.Length(50));
				productClass.Property(x => x.Description, prop => prop.Length(1000));
                productClass.Property(x => x.UnitsInStock);
				productClass.ManyToOne(x => x.Supplier);
				productClass.ManyToOne(x => x.Category);
			});

			mapper.Class<Supplier>(manufacturerClass =>
			{
				manufacturerClass.Property(x => x.Code, prop => prop.Length(20));
				manufacturerClass.Property(x => x.Name, prop => prop.Length(50));
			});

			mapper.Class<Category>(productTypeClass =>
			{
				productTypeClass.Property(x => x.Code, prop => prop.Length(20));
				productTypeClass.Property(x => x.Name, prop => prop.Length(50));
			});

			configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
		}

		private static void MapClass(IModelInspector modelinspector, Type type, IClassAttributesMapper mapper)
		{
			if (typeof(Entity).IsAssignableFrom(type))
			{
				mapper.Id(type.GetProperty("Id"), id =>
				{
					id.Column(type.Name + "Id");
					id.Generator(Generators.GuidComb);
					id.UnsavedValue(Guid.Empty);
				});
			}
		}

		private static void MapManyToOne(IModelInspector modelinspector, PropertyPath member, IManyToOneMapper mapper)
		{
			var columnName = member.LocalMember.GetPropertyOrFieldType().Name + "Id";
			mapper.Column(columnName);
			string foreignKey = string.Format("FK_{0}_{1}_{2}", member.LocalMember.ReflectedType.Name, member.LocalMember.Name,
				columnName);
			mapper.ForeignKey(foreignKey);
			mapper.Cascade(Cascade.All);
			mapper.NotNullable(true);
		}

		private static void MapProperty(IModelInspector modelinspector, PropertyPath member, IPropertyMapper mapper)
		{
			mapper.NotNullable(true);
		}
 
	}
}