using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using TinyNH.DemoStore.Core.Domain.NHibernate;
using NHibernate.Cfg;

namespace TinyNH.DemoStore.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SetupNHibernate();
        }

        private void SetupNHibernate()
        {
            var builder = new ConfigurationBuilder(c =>
                c.CurrentSessionContext<LazyWebSessionContext>());
            ConfigurationStore = new ConfigurationStore(builder.Build);
            LazyWebSessionContextModule.ConfigurationStore = ConfigurationStore;
        }

        public static ConfigurationStore ConfigurationStore { get; private set; }

        /// <summary>
        /// Gets the current NHibernate session made available for the current web request
        /// </summary>
        /// <remarks>
        /// In this demo, we're choosing to make the current session available via a 
        /// global property - there are other options that might scale better in a 
        /// larger application, e.g. an IOC container.
        /// </remarks>
        public static ISession CurrentSession
        {
            get { return ConfigurationStore.SessionFactory.GetCurrentSession(); }
        }
    }
}