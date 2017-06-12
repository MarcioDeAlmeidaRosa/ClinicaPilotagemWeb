using System;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using ClinicaPilotagemWeb.Models;
using ClinicaPilotagemWeb.Models.Responses.Authentication;
using System.IO;
using System.Net.Http.Formatting;
using System.Net;

namespace ClinicaPilotagemWeb.Controllers
{
    public class AuthenticationController : BaseControllerController
    {
        private const int APLICATION = 1;/*app clinica de pilotagem*/

        [HttpGet]
        //[AllowAnonymous]
        public ActionResult Logout()
        {
            //TODO - EFETUAR LOGOUT E ENCAMINHA PARA A TELA INICIAL
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new UserModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserModel model, string returnUrl)
        {
            //TODO - VERIFICAR REGRA DE returnUrl
            if (!ModelState.IsValid)
                return View(model);

            ResultAutentication userAuthentication = null;

            var user = new { Email = model.Email, Password = model.Password, Aplication = APLICATION };

            // HTTP POST
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["url-api-client"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Authentication/Login", user);
                if (!response.IsSuccessStatusCode)
                {
                    var critica = response.Content.ReadAsAsync<ValidationError>().Result;
                    if (critica != null)
                        ViewBag.MessageError = critica.Message;
                    else
                        ViewBag.MessageError = Resources.Language.ErrorTryLogIn;
                    return View(model);
                }

                userAuthentication = await response.Content.ReadAsAsync<ResultAutentication>();
                TOKEN = userAuthentication.Token;
            }

            FormsAuthentication.SetAuthCookie(model.Email, false);
            var authTicket = new FormsAuthenticationTicket(1, userAuthentication.User.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, Convert.ToString(user.Aplication));
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
            return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistrationModel model, string returnUrl)
        {
            //TODO - VERIFICAR REGRA DE returnUrl
            if (!ModelState.IsValid)
                return View(model);

            ResultAutentication userAuthentication = null;

            var user = new { UserName = model.Name, Email = model.Email, Password = model.Password, Aplication = APLICATION };

            // HTTP POST
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["url-api-client"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Authentication/Register", user);
                if (!response.IsSuccessStatusCode)
                {
                    var critica = response.Content.ReadAsAsync<ValidationError>().Result;
                    if (critica != null)
                        ViewBag.MessageError = critica.Message;
                    else
                        ViewBag.MessageError = Resources.Language.ErrorTryRegister;
                    return View(model);
                }

                ViewBag.MessageSuccess = Resources.Language.SuccessRegister;

                return View("Login");// RedirectToAction("Login", "Authentication");
            }
        }
    }
}