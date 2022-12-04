using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Summary.WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "sign-in",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Summary.WebApi.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "sign-up",
                defaults: new { controller = "Login", action = "Register", id = UrlParameter.Optional },
                namespaces: new string[] { "Summary.WebApi.Controllers" }
            );

            routes.MapRoute(
                name: "Update-Account",
                url: "update-account",
                defaults: new { controller = "Login", action = "Register", id = UrlParameter.Optional },
                namespaces: new string[] { "Summary.WebApi.Controllers" }
            );

            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Login", action = "ManageAccount", id = UrlParameter.Optional },
                 namespaces: new string[] { "Summary.WebApi.Controllers" }
             );
        }
    }
}
