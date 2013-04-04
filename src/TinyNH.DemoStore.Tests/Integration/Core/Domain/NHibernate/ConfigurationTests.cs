using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain.NHibernate;
using TinyNH.DemoStore.Core.Infrastructure;

namespace TinyNH.DemoStore.Tests.Integration.Core.Domain.NHibernate
{
	[TestFixture]
	public class ConfigurationTests
	{
		[Test]
		public void configuration_and_generated_schema_should_be_valid()
		{
            DatabaseSetUpHelper.RecreateDatabase(Environment.IntegrationTests);
			var configuration = new ConfigurationBuilder().Build();
			var sessionFactory = configuration.BuildSessionFactory();
            new SchemaExport(configuration).SetOutputFile("..\\..\\Generated\\schema.sql").Create(true, true);
		}


	}
}