using System.Web.Mvc;

namespace TinyNH.DemoStore.Admin.Controllers.Home
{
    public class HomeController : AppController
    {
         public ActionResult Index()
         {
             return View();
         }
    }
}