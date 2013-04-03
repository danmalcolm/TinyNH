using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using TinyNH.DemoStore.Core.Domain;
using TinyNH.DemoStore.Core.Infrastructure;

namespace TinyNH.DemoStore.Admin.Controllers.DatabaseSetUp
{
    public class DatabaseSetUpController : AppController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Not for production!
            if(ConfigurationUtility.ReadAppSetting("Environment") != "Dev")
            {
                filterContext.Result = new HttpUnauthorizedResult("");
            }
        }

        public ActionResult Index()
        {
            bool ready = true;
            string message = "";
            try
            {
                int productCount = CurrentSession.Query<Product>().Count();
            } 
            catch(Exception exception)
            {
                ready = false;
                message = exception.ToString();
            }
            return View(new DatabaseInfo { Ready = ready, Message = message });
        } 

        [HttpPost]
        public ActionResult Setup()
        {
            CreateDatabaseAndSchema();

            Seeder.SeedDatabase(CurrentSession);
            
            FlashMessage("The database was set up successfully");
            return Redirect("Index");
        }

        private void CreateDatabaseAndSchema()
        {
            DatabaseSetUpHelper.RecreateLocalDevDatabase();
            new SchemaExport(MvcApplication.ConfigurationStore.Configuration).Create(false, true);
        }
    }
}