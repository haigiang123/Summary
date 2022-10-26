using Microsoft.AspNet.Identity.Owin;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Summary.WebApi.App_Start;
using Summary.WebApi.Infrastructure.Core;
using Summary.Business;
using Summary.Share.ViewModel;

namespace Summary.WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiControllerBase
    {
        private IErrorBusiness _errorBusiness;    

        public AccountController(IErrorBusiness errorBusiness) : base(errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        [HttpPost]
        [Route("login")]
        public async Task<HttpResponseMessage> Login(HttpRequestMessage request, PostVM vn)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(vn.Name, vn.Alias, true, shouldLockout: false);
            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
