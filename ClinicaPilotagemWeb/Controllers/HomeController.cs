using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public class HomeController : BaseControllerController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Fone = ContactFoneNumber;
            ViewBag.ContactEmailSupport = ContactEmailSupport;
            ViewBag.ContactEmailMarketing = ContactEmailMarketing;
            ViewBag.ContactEnderecoLinha1 = ContactEnderecoLinha1;
            ViewBag.ContactEnderecoLinha2 = ContactEnderecoLinha2;
            return View();
        }
    }
}