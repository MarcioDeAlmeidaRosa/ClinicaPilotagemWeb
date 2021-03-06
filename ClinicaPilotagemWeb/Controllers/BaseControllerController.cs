﻿using System;
using System.Configuration;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClinicaPilotagemWeb.Helper;
using ClinicaPilotagemWeb.Models;
using ClinicaPilotagemWeb.Models.Responses.Authentication;

namespace ClinicaPilotagemWeb.Controllers
{
    public abstract class BaseControllerController : Controller
    {
        protected const int APLICATION = 1;/*app clinica de pilotagem*/

        protected static string ContactFoneNumber;
        protected static string ContactEmailSupport;
        protected static string ContactEmailMarketing;
        protected static string ContactEnderecoLinha1;
        protected static string ContactEnderecoLinha2;
        protected string cultureName;
        protected string TOKEN;

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
            cultureName = RouteData.Values["culture"] as string;

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

        protected void SetAuthCookie(UserModel model, ResultAutentication userAuthentication)
        {
            FormsAuthentication.SetAuthCookie(model.Email, false);
            var authTicket = new FormsAuthenticationTicket(1, userAuthentication.User.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, Convert.ToString(APLICATION));
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
        }

        protected void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}