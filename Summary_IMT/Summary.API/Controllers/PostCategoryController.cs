using AutoMapper;
using Summary.Business;
using Summary.Share.ViewModel;
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
        private readonly IMapper _mapper;

        public PostCategoryController(IErrorBusiness errorService, IMapper mapper, IPostCategoryBusiness postCategoryBusiness) :
            base(errorService)
        {
            this._postCategoryBusiness = postCategoryBusiness;
            this._mapper = mapper;
        }

        [Route("getall")]
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