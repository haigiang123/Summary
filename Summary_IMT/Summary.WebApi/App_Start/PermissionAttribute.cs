using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Summary.Share.Helper;
using System.Web.Routing;
using Summary.Business;

namespace Summary.WebApi.App_Start
{
    public class PermissionAttribute : ActionFilterAttribute
    {
       public PermissionAttribute(PermissionObject obj, PermissionAction action)
        {
            Obj = obj;
            Action = action;
        }

        public PermissionObject Obj { get; set; }

        public PermissionAction Action { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext.GetOwinContext().Authentication;
            if (context.User.Identity.IsAuthenticated)
            {
                var userPermission = filterContext.HttpContext.GetOwinContext().Authentication.User.FindFirst("permissions").Value;
                
                byte[] permission = JsonConvert.DeserializeObject<byte[]>(userPermission);
                if ((permission[(byte)Obj] & (byte)Action) != (byte)Action)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "info",
                        controller = "login",
                        message = "You don't have permission to access this action"
                    }));
                }  
            }
        }
    }
}