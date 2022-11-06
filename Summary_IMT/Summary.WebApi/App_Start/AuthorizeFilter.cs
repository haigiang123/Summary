using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Summary.WebApi.App_Start
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var method = filterContext.HttpContext.Request.HttpMethod;
                if (string.Equals("GET", method, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    string antiToken = filterContext.HttpContext.Request.Headers["__RequestVerificationToken"] ?? "";
                    var cookie = filterContext.HttpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                    AntiForgery.Validate(cookie != null ? cookie.Value : null, antiToken);
                }
                
            }
            catch (HttpAntiForgeryException ex)
            {
                filterContext.Result = new HttpStatusCodeResult(400, ex.Message);
            }
        }
    }
}