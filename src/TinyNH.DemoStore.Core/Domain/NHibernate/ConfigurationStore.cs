using System;
using NHibernate;
using NHibernate.Cfg;
using TinyNH.DemoStore.Core.Infrastructure;

namespace TinyNH.DemoStore.Core.Domain.NHibernate
{
	/// <summary>
	/// Provides access to NHibernate Configuration and ISessionFactory used to load and
	/// save an object model within the application. Ensures that the ISessionFactory 
	/// is built only once during the lifetime of the application.
	/// </summary>
	public class ConfigurationStore
	{
		private readonly ThreadSafeInitializer<Configuration> configurationInitializer;
		private readonly ThreadSafeInitializer<ISessionFactory> factoryInitializer;

		public ConfigurationStore(Func<Configuration> create)
		{
			configurationInitializer = ThreadSafeInitializer.Create(create);
			factoryInitializer = ThreadSafeInitializer.Create(() => Configuration.BuildSessionFactory());
		}

		public Configuration Configuration
		{
			get { return configurationInitializer.Value; }
		}

		public ISessionFactory SessionFactory
		{
			get { return factoryInitializer.Value; }
		}
	}
}