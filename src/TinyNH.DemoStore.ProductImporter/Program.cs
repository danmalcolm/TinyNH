using System;
using NHibernate.Tool.hbm2ddl;
using TinyNH.DemoStore.Core.Domain.NHibernate;
using TinyNH.DemoStore.Core.Infrastructure;

namespace TinyNH.DemoStore.ProductImporter
{
	class Program
	{
		static void Main(string[] args)
		{
			var configurationStore = new ConfigurationStore(new ConfigurationBuilder().Build);
		    new Importer(configurationStore.SessionFactory).Execute();
			Console.WriteLine("Import Complete. Press a key to exit");
			Console.ReadKey();
		}
	}
}
