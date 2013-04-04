using System;
using System.Collections.Generic;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using TinyNH.DemoStore.Core.NHibernate;

namespace TinyNH.DemoStore.Core.Domain.NHibernate
{
    /// <summary>
    /// Creates NHibernate Configuration for the application's object model
    /// </summary>
	public class ConfigurationBuilder
    {
        private readonly Action<Configuration> customize;
        
        /// <param name="customize">An optional action to customize the configuration after it has been built with standard settings. Intended to allow settings to be added when running tests, such as enabling statistics or logging sql</param>
        public ConfigurationBuilder(Action<Configuration> customize = null)
		{
		    this.customize = customize;
		}
		
		public Configuration Build()
		{
			var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
			{
				db.Dialect<MsSql2008Dialect>();
				db.ConnectionStringName = "core";
				db.BatchSize = 100;
			});
			configuration.CurrentSessionContext<CallSessionContext>();
			
            // example of adding other properties
			configuration.AddProperties(new Dictionary<string, string> { { "generate_statistics", "false" } });
			
			ModelMapping.Add(configuration);

            if (customize != null)
                customize(configuration);

			return configuration;
		}
	}
}