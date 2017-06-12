using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using ClinicaPilotagemWeb.Models;
using ClinicaPilotagemWeb.Models.Responses.Authentication;

namespace ClinicaPilotagemWeb.Controllers
{
    public class AuthenticationController : BaseControllerController
    {
        [HttpGet]
        //[AllowAnonymous]
        public ActionResult Logout()
        {
            base.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        //[AllowAnonymous]
        public ActionResult GetIn()
        {
            return RedirectToAction("Index", "Main");
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

            //Set usuário logado
            SetAuthCookie(model, userAuthentication);

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