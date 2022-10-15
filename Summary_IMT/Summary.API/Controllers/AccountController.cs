using Microsoft.AspNet.Identity.Owin;
using Summary.API.App_Start;
using Summary.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Summary.API.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController: APIControllerBase
    {
        private readonly IErrorBusiness _errorBusiness;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AccountController(IErrorBusiness errorBusiness) : base(errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<HttpResponseMessage> Login(HttpRequestMessage request)
        {
            var result = await SignInManager.PasswordSignInAsync("admin", "123654$", true, true);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}