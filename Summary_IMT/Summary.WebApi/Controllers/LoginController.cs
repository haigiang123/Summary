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
using Summary.Business;
using Newtonsoft.Json;

namespace Summary.WebApi.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationUserManager _applicationUserManager;
        private ApplicationSignInManager _applicationSignInManager;
        private IAppRolePermissionBusiness _appRolePermissionBusiness;

        public LoginController(ApplicationUserManager applicationUserManager, 
            ApplicationSignInManager applicationSignInManager,
            IAppRolePermissionBusiness appRolePermissionBusiness)
        {
            _applicationUserManager = applicationUserManager;
            _applicationSignInManager = applicationSignInManager;
            _appRolePermissionBusiness = appRolePermissionBusiness;
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

        public IAuthenticationManager authenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #region Login and logout
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login(LoginVM login)
        {
            try
            {
                login.UserName = "admin";
                login.Password = "123456aA@";
                if (ModelState.IsValid)
                {
                    AppUser user = await UserManager.FindByNameAsync(login.UserName);
                    var signInStatus = await SignInManager.PasswordSignInAsync(login.UserName, login.Password, login.Remember, true);
                    if(user != null)
                    {
                        //var permission = new SummaryPrincipal(login.UserName, a);

                        switch (signInStatus)
                        {
                            case SignInStatus.Success:
                                await SignInAsync(user, login.Remember);
                                return RedirectToAction("ManageAccount");
                            case SignInStatus.LockedOut:
                                if (user.LockoutEndDateUtc <= DateTime.Now)
                                {
                                    await UserManager.ResetAccessFailedCountAsync(user.Id);
                                    UserManager.UserLockoutEnabledByDefault = false;
                                    await UserManager.SetLockoutEnabledAsync(user.Id, false);
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    return RedirectToAction("Info");
                                }
                            case SignInStatus.Failure:
                                UserManager.MaxFailedAccessAttemptsBeforeLockout = 5; // max fail attemps  
                                //UserManager.SetLockoutEnabled(user.Id, true);
                                UserManager.UserLockoutEnabledByDefault = true;
                                await UserManager.AccessFailedAsync(user.Id);

                                return RedirectToAction("Index");
                            default: 
                                return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                    //ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    //AuthenticationProperties authenticationProperties = new AuthenticationProperties();

                    //IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                    //authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //authenticationProperties.IsPersistent = login.Remember;
                    //authentication.SignIn(authenticationProperties, identity);

                    //return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");
        }

        public async Task SignInAsync(AppUser appUser, bool isPersistent)
        {
            var a = _appRolePermissionBusiness.GetPermissionByUserId(appUser.Id);
            var user = await appUser.GenerateUserIdentityAsync(UserManager);
            user.AddClaim(new Claim("permissions", JsonConvert.SerializeObject(a)));

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent },
                user);
        }

        [Authorize]
        public ActionResult Logout()
        {

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);

            return RedirectToAction("Index", "Login");
        }
        #endregion

        #region register with an email confirm attached Token
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
        #endregion

        #region Test ajax and javascript as the form basic, integrate TinyUpload file
        //[AllowAnonymous]
        [PermissionAttribute(PermissionObject.Admin, PermissionAction.Delete)]
        public ActionResult UpdateAccount()
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

        #endregion

        #region Manage Account

        [Authorize]
        public async Task<ActionResult> ManageAccount(ManageMessageId? mesId)
        {
            ViewBag.Message = mesId == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : mesId == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : mesId == ManageMessageId.SetTwoFactorSuccess ? "Your two factor provider has been set."
                : mesId == ManageMessageId.Error ? "An error has occurred."
                : mesId == ManageMessageId.AddPhoneSuccess ? "The phone number was added."
                : mesId == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();

            ManageAccountVM model = new ManageAccountVM
            {
                AccountId = userId,
                HasPassword = await UserManager.HasPasswordAsync(userId),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId)
            };

            return View(model);
        }

        [Authorize]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberVM req)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("PhoneNumber", "Sai rồi");
                return View(req);
            }

            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), req.PhoneNumber);
            if(UserManager.SmsService != null)
            {
                try
                {
                    await UserManager.SmsService.SendAsync(new IdentityMessage
                    {
                        Destination = req.PhoneNumber,
                        Body = $"Your security code is: {code}"
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }

            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = req.PhoneNumber });
        }

        [Authorize]
        public async Task<ActionResult> VerifyPhoneNumber(string PhoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), PhoneNumber);
            ViewBag.Status = "For DEMO purposes only, the current code is " + code;
            return PhoneNumber == null ? View("Error") : View(new VerifyPhoneNumberVM { PhoneNumber = PhoneNumber });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberVM req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }

            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), req.PhoneNumber, req.Code);
            if (result.Succeeded)
            {
                var user = UserManager.FindByIdAsync(User.Identity.GetUserId());

                await SignInAsync(user.Result, false);

                return RedirectToAction("ManageAccount", new { mesId = ManageMessageId.AddPhoneSuccess });
            }
            

            return View();
        }

        [Authorize]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var userId = User.Identity.GetUserId();
            var result = await UserManager.SetPhoneNumberAsync(userId, null);

            if (!result.Succeeded)
            {
                return RedirectToAction("ManageAccount", new { mesId = ManageMessageId.Error });
            }

            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("ManageAccount", new { mesId = ManageMessageId.RemovePhoneSuccess });
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordVM req)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if(await UserManager.CheckPasswordAsync(user, req.OldPassword) == false)
            {
                ModelState.AddModelError("", "User isn't exists");
                return View(req);
            }

            var result = await UserManager.ChangePasswordAsync(user.Id, req.OldPassword, req.NewPassword);
            if (result.Succeeded)
            {
                user = await UserManager.FindByIdAsync(user.Id);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("ManageAccount", new { mesId = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);
            return View(req);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            SetTwoFactorSuccess,
            Error,
            AddPhoneSuccess,
            RemovePhoneSuccess
        }

        #endregion
    }
}