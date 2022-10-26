using Summary.WebApi.Models;
using Summary.Model.Models;
using Summary.WebApi.App_Start;

using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Summary.Share.Helper;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;

namespace Summary.WebApi.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationUserManager _applicationUserManager;
        private ApplicationSignInManager _applicationSignInManager;

        public LoginController(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager)
        {
            _applicationUserManager = applicationUserManager;
            _applicationSignInManager = applicationSignInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _applicationSignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _applicationSignInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _applicationUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _applicationUserManager = value; }
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login(LoginVM login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppUser user = await UserManager.FindAsync(login.UserName, login.Password);
                    if (user != null)
                    {
                        ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationProperties authenticationProperties = new AuthenticationProperties();

                        IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                        authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        authenticationProperties.IsPersistent = login.Remember;
                        authentication.SignIn(authenticationProperties, identity);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Account or UserName is incorrect");
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View(login);
        }

        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = false,
                    BirthDay = DateTime.Now,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Login",
                       new
                       {
                           userId = user.Id,
                           code = code
                       }, protocol: Request.Url.Scheme);

                    UserManager.EmailService = new EmailService();

                    //ConfigurationHelper.GetByKey("CurrentLink") + "/login"
                    string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/newuser.html"));
                    content = content.Replace("{{UserName}}", "haigt");
                    content = content.Replace("{{Link}}", callbackUrl);

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", content);

                    var message = "Check your email and confirm your account, you must be confirmed "
                             + "before you can log in.";

                    return RedirectToAction("Info", new { message = message });
                }
            }
            return View(model);
        }

        public ActionResult Info(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}