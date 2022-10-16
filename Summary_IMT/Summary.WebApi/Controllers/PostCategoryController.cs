using AutoMapper;
using Summary.Business;
using Summary.Share.ViewModel;
using Summary.WebApi.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Summary.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/postcategory")]
    public class PostCategoryController : ApiControllerBase
    {
        private IPostCategoryBusiness _postCategoryBusiness;
        private readonly IMapper _mapper;

        public PostCategoryController(IErrorBusiness errorService, IMapper mapper
            , IPostCategoryBusiness postCategoryBusiness) :
            base(errorService)
        {
            this._postCategoryBusiness = postCategoryBusiness;
            this._mapper = mapper;
        }

        [Route("getall")]
        [HttpPost]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listCategory = this._postCategoryBusiness.GetAll().ToList();

                var listPostCategoryVm = _mapper.Map<List<PostCategoryVM>>(listCategory);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listPostCategoryVm);

                return response;
            });
        }
    }
}
