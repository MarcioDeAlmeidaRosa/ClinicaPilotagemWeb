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
            if (!ModelState.IsValid)
                return View(model);

            StatusLogin status = StatusLogin.Failure;
            ResultAutentication userAuthentication = null;

            var user = new { Email = model.Email, Password = model.Password, Aplication = APLICATION };

            // HTTP POST
            //https://sites.google.com/site/wcfpandu/web-api/calling-a-web-api-from-c-and-calling-a-web-api-from-view
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["url-api-client"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Authentication/Login", user);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("authentication", Resources.Language.ErrorTryLogIn);
                    return View(model);
                }

                userAuthentication = await response.Content.ReadAsAsync<ResultAutentication>();
                TOKEN = userAuthentication.Token;
                status = userAuthentication.StatusLogin;
            }

            switch (status)
            {
                case StatusLogin.Success:
                    //Session[]
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    var authTicket = new FormsAuthenticationTicket(1, userAuthentication.User.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, Convert.ToString(user.Aplication));
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Main");
                default:
                    if (status == StatusLogin.Failure)
                        ModelState.AddModelError("authentication", Resources.Language.UserNotFound);
                    else if (status == StatusLogin.LockedOut)
                        ModelState.AddModelError("authentication", Resources.Language.UserIsBlocked);
                    else if (status == StatusLogin.RequiresVerification)
                        ModelState.AddModelError("authentication", Resources.Language.UserConfirm);

                    return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
    }
}