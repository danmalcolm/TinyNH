using System;
using TinyNH.DemoStore.Core.Domain.NHibernate;

namespace TinyNH.DemoStore.ProductImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().Build();
            var sessionFactory = configuration.BuildSessionFactory();
            new Importer(sessionFactory).Execute();
            Console.WriteLine("Import Complete. Press a key to exit");
            Console.ReadKey();
        }
    }
}
