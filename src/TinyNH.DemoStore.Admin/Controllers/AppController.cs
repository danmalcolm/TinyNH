using System.Web.Mvc;
using NHibernate;

namespace TinyNH.DemoStore.Admin.Controllers
{
    /// <summary>
    /// Base class with some shared application-specific functionality
    /// </summary>
	public abstract class AppController : Controller
	{
		protected ISession CurrentSession
		{
			get { return MvcApplication.CurrentSession; }
		}

	    protected void FlashMessage(string message)
	    {
	        TempData.Add("flash-message", message);
	    }
	}
}