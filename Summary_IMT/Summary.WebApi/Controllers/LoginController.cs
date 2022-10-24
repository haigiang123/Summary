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
                    AppUser user = await _applicationUserManager.FindAsync(login.UserName, login.Password);
                    if(user != null)
                    {
                        ClaimsIdentity identity = _applicationUserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
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
            catch(Exception ex)
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
        public async Task<ActionResult> Register(RegisterVM model)
        {

            if (ModelState.IsValid)
            {
                var userByEmail = await _applicationUserManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email đã tồn tại");
                    return View(model);
                }
                var userByUserName = await _applicationUserManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("email", "Tài khoản đã tồn tại");
                    return View(model);
                }
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address

                };

                await _applicationUserManager.CreateAsync(user, model.Password);

                var adminUser = await _applicationUserManager.FindByEmailAsync(model.Email);
                if (adminUser != null)
                    await _applicationUserManager.AddToRolesAsync(adminUser.Id, new string[] { "Member" });

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/newuser.html"));
                content = content.Replace("{{UserName}}", adminUser.FullName);
                content = content.Replace("{{Link}}", ConfigurationHelper.GetByKey("CurrentLink") + "/login");

                MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);


                ViewData["SuccessMsg"] = "Đăng ký thành công";
            }

            return View();
        }

    }
}