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
using System.IO;
using System.Linq;

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

        [AllowAnonymous]
        public async Task<ActionResult> UpdateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestAjax(IntegrateTinyMCEVM model)
        {
            var mess = new { status = 1, message = "OKIE" };   
            return Json( mess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //HttpPostedFileBase[] content
        public async Task<ActionResult> UpdateAccount(IntegrateTinyMCEVM model)
        {
            foreach (var item in model.Image)
            {
                //var file = Request.Files["Image"];
                string extension = Path.GetExtension(item.FileName);
                string fileid = Guid.NewGuid().ToString();
                fileid = Path.ChangeExtension(fileid, extension);

                string savePath = Server.MapPath(@"~\Uploads\" + fileid);
                item.SaveAs(savePath);
            }


            //return Content(Url.Content(@"~\Uploads\" + fileid));

            return View("DisplayNewContent", model);
        }

        //public ActionResult TinyMceUpload()
        //{
        //    var file = Request.Files["file"];

        //    string extension = Path.GetExtension(file.FileName);
        //    string fileid = Guid.NewGuid().ToString();
        //    fileid = Path.ChangeExtension(fileid, extension);

        //    var draft = new { location = "" };

        //    if (file != null && file.ContentLength > 0)
        //    {
        //        const int megabyte = 1024 * 1024;

        //        if (!file.ContentType.StartsWith("image/"))
        //        {
        //            throw new InvalidOperationException("Invalid MIME content type.");
        //        }

        //        string[] extensions = { ".gif", ".jpg", ".png" };
        //        if (!extensions.Contains(extension))
        //        {
        //            throw new InvalidOperationException("Invalid file extension.");
        //        }

        //        if (file.ContentLength > (8 * megabyte))
        //        {
        //            throw new InvalidOperationException("File size limit exceeded.");
        //        }

        //        string savePath = Server.MapPath(@"~/Uploads/" + fileid);
        //        file.SaveAs(savePath);

        //        draft = new { location = Path.Combine("/Uploads", fileid).Replace('\\', '/') };
        //    }


        //    return Json(draft, JsonRequestBehavior.AllowGet);
        //} 

    }
}