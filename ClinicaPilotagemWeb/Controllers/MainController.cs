using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public class MainController : BaseControllerController
    {
        // GET: Main
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}