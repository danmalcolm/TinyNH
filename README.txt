TinyNH - The Small Footprint NHibernate Infrastructure
======================================================

TinyNH is designed to help you get your project up and running with a simple solid NHibernate persistence infrastructure. 

It isn’t a framework, it’s more like a template project that demonstrates some key infrastructural elements. Each element is accompanied by an article that describes what it does and why, helps you to decide whether or not you need it and, if so, tells you how to add it to your project. 

For further details, please see the series of accompanying blog articles at:

TODO


Development Environment
-----------------------

The solution is compatible with Visual Studio 2012 only. It includes an ASP.Net MVC v4 web application project.


SQL Server Setup
----------------

The solution uses SQL Server Express LocalDB, which should be installed if you have Visual Studio 2012 (open a command prompt and type "where SQLLocalDB" - it should be somewhere like "C:\Program Files\Microsoft SQL Server\110\Tools\Binn\SqlLocalDB.exe").

LocalDB is a lightweight version of SQL Server Express that is ideal for running database applications in a development environment. You can browse a LocalDB instance's databases and their objects via SQL Server Management Studio or Visual Studio 2012's SQL Server Object Explorer. Just connect to instance "(localDB)\v11.0", one of the default LocalDB instances.

All connection strings in this project use LocalDB instance "(localDB)\v11.0". If you use a different SQL Server for development, modify the connection strings in the following files accordingly:

- TinyNH.DemoStore.Admin\web.config
- TinyNH.DemoStore.ProductImporter\app.config
- TinyNH.DemoStore.Tests\app.config


Warning - Automated Database Setup Ahead!
-----------------------------------------

You don't need to create any databases yourself to run the solution. TinyNH demonstrates ways of automating the creation of development and test databases. The credentials that you specify in your "setup" connection strings will need to have permission to drop and create databases on your development SQL Server. This shouldn't be a problem with LocalDB but it's something to be aware of if you're using a different server.

One more thing... Running the applications and tests within the solution will drop and recreate databases TinyNH.DemoStore.Dev and TinyNH.DemoStore.Tests. If you decided on a whim to implement a web framework called "SQL on Speed" with some funky stored procedures and you happened to do this within a TinyNH database becuase it was there, and then you happened to run TinyNH again you might lose all your work. 




