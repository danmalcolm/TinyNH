using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using TinyNH.DemoStore.Core.Domain.NHibernate;
using TinyNH.DemoStore.Core.Infrastructure;

namespace TinyNH.DemoStore.Tests.Integration.Core.Domain.NHibernate
{
    [TestFixture]
    public abstract class PersistenceTestsBase
    {
        protected static ConfigurationStore ConfigurationStore { get; private set; }

        static PersistenceTestsBase()
        {
            // Initialise configuration shared throughout test run
            var builder = new ConfigurationBuilder(c =>
            {
                c.DataBaseIntegration(db =>
                {
                    db.LogFormattedSql = true;
                    db.LogSqlInConsole = true;
                });
                c.Properties["generate_statistics"] = "true";
                
                // Generate schema ready for tests to run
                DatabaseSetUpHelper.RecreateIntegrationTestsDatabase();
                new SchemaExport(c).Create(false, true);
            });
            
            ConfigurationStore = new ConfigurationStore(builder.Build);
        }

        [SetUp]
        public void SetUp()
        {
            InTransaction(ResetDatabase);
        }
        
        // Removes all data from test database, leaving it in a clean state for next test
        public static void ResetDatabase(ISession session)
        {
            const string resetSql = @"delete from dbo.Product
delete from dbo.Supplier
delete from dbo.Category";

            var command = session.Connection.CreateCommand();
            command.CommandText = resetSql;
            session.Transaction.Enlist(command);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Starts a session and transaction and executes the specified action
        /// </summary>
        /// <param name="action"></param>
        protected void InTransaction(Action<ISession> action)
        {
            using (var session = ConfigurationStore.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                action(session);
                transaction.Commit();
            }
        }


    }
}