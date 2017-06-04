using ClinicaPilotagemWeb.Models;
using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public class AutenticationController : BaseControllerController
    {
        [HttpGet]
        //[AllowAnonymous]
        public ActionResult Logout()
        {
            //TODO - EFETUAR LOGOUT E ENCAMINHA PARA A TELA INICIAL
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User entity)
        {
            if (ModelState.IsValid)
            {
                //TODO - CHAMA SERVIÇO PARA EXECUTAR LOGIN

                return RedirectToAction("Index", "Main");
            }

            return View(entity);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
    }
}