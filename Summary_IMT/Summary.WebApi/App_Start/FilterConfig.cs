using Summary.WebApi.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Summary.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeFilter());
        }
    }
}
