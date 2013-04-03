using System.Collections.Generic;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain.NHibernate;
using TinyNH.DemoStore.Core.Infrastructure;
using NHibernate.Cfg;

namespace TinyNH.DemoStore.Tests.Integration.Core.Domain.NHibernate
{
	[TestFixture]
	public class ConfigurationBuilderTests
	{
		[Test]
		public void should_use_default_properties_if_no_config_file_specified()
		{
			var configuration = new ConfigurationBuilder().Build();
			Assert.AreEqual("core", configuration.Properties["connection.connection_string_name"]);
		}
		
		[Test]
		public void should_customize_default_configuration_if_specified()
		{
			var configuration = new ConfigurationBuilder(c =>
			{
			    c.Properties["generate_statistics"] = "true";
			    c.DataBaseIntegration(db => db.ConnectionStringName = "core2");
			})
                .Build();
			Assert.AreEqual("true", configuration.Properties["generate_statistics"]);
			Assert.AreEqual("core2", configuration.Properties["connection.connection_string_name"]);
		}
		
		[Test]
		public void configuration_and_generated_schema_should_be_valid()
		{
            DatabaseSetUpHelper.RecreateIntegrationTestsDatabase();
			var configuration = new ConfigurationBuilder().Build();
			var sessionFactory = configuration.BuildSessionFactory();
            new SchemaExport(configuration).SetOutputFile("..\\..\\Generated\\schema.sql").Create(true, true);
		}


	}
}