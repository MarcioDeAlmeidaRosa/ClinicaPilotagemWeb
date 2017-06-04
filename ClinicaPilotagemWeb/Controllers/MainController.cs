using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public class MainController : BaseControllerController
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
    }
}