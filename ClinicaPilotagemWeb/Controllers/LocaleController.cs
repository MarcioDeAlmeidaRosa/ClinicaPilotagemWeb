using System.Threading;
using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public class LocaleController : BaseControllerController
    {
        public ActionResult CurrentCulture()
        {
            return Json(/*Thread.CurrentThread.CurrentUICulture.ToString()*/"pt-BR", JsonRequestBehavior.AllowGet);
        }
    }
}