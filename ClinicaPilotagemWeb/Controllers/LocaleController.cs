using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public class LocaleController : BaseControllerController
    {
        public ActionResult CurrentCulture()
        {
            return Json(cultureName, JsonRequestBehavior.AllowGet);
        }
    }
}