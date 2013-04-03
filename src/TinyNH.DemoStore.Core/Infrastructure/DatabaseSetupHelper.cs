using System;
using System.Data.SqlClient;

namespace TinyNH.DemoStore.Core.Infrastructure
{
	public class DatabaseSetUpHelper
	{
	    public const string LocalDevDbName = "TinyNH.DemoStore.Dev";
	    public const string IntegrationTestsDbName = "TinyNH.DemoStore.IntegrationTests";

        public static void RecreateLocalDevDatabase()
        {
            RecreateDatabase(LocalDevDbName);
        }

        public static void RecreateIntegrationTestsDatabase()
        {
            RecreateDatabase(IntegrationTestsDbName);
        }

		public static void RecreateDatabase(string databaseName)
		{
			const string commandTemplate = @"
if exists(select * from sysdatabases where name = '{0}')
begin
	alter database [{0}] set offline with rollback immediate 
	alter database [{0}] set online 
	drop database [{0}]
end
	
create database [{0}]";

			string command = string.Format(commandTemplate, databaseName);
			ExecuteSql(command, databaseName);
		}
        
		private static void ExecuteSql(string commandText, string databaseName)
		{
			var connectionString = ConfigurationUtility.ReadConnectionString("setup");
			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (var command = new SqlCommand(commandText, connection))
					{
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("An error occurred while trying to create the test database '{0}'", databaseName), e);
			}
		}
	}
}