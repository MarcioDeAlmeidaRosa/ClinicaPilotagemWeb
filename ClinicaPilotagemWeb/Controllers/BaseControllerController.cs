using ClinicaPilotagemWeb.Helper;
using System;
using System.Configuration;
using System.Threading;
using System.Web.Mvc;

namespace ClinicaPilotagemWeb.Controllers
{
    public abstract class BaseControllerController : Controller
    {
        protected static string ContactFoneNumber;
        protected static string ContactEmailSupport;
        protected static string ContactEmailMarketing;
        protected static string ContactEnderecoLinha1;
        protected static string ContactEnderecoLinha2;

        static BaseControllerController()
        {
            ContactFoneNumber = ConfigurationManager.AppSettings["ContactFone"];
            ContactEmailSupport = ConfigurationManager.AppSettings["ContactEmailSupport"];
            ContactEmailMarketing = ConfigurationManager.AppSettings["ContactEmailMarketing"];
            ContactEnderecoLinha1 = ConfigurationManager.AppSettings["ContactEnderecoLinha1"];
            ContactEnderecoLinha2 = ConfigurationManager.AppSettings["ContactEnderecoLinha2"];
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = RouteData.Values["culture"] as string;

            if (cultureName == null)
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            cultureName = CultureHelper.GetImplementedCulture(cultureName); // Veja mais abaixo na resposta

            if (RouteData.Values["culture"] as string != cultureName)
            {
                // Força uma cultura válida na URL
                RouteData.Values["culture"] = cultureName.ToLowerInvariant();
                Response.RedirectToRoute(RouteData.Values);
            }

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
}