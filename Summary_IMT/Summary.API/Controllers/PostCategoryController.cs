using Summary.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Summary.API.Controllers
{
    [RoutePrefix("api/postcategory")]
    public class PostCategoryController : APIControllerBase
    {
        private IPostCategoryBusiness _postCategoryBusiness;

        public PostCategoryController(IErrorBusiness errorService, IPostCategoryBusiness postCategoryBusiness) :
            base(errorService)
        {
            this._postCategoryBusiness = postCategoryBusiness;
        }

        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listCategory = this._postCategoryBusiness.GetAll();

                // var listPostCategoryVm = Mapper.Map<List<PostCategoryViewModel>>(listCategory);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listCategory);

                return response;
            });
        }
    }
}