using Microsoft.AspNet.Identity.Owin;
using Summary.Business;
using Summary.Model.Models;
using Summary.WebApi.App_Start;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace Summary.WebApi.Infrastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        private IErrorBusiness _errorBusiness;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

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

        // GET: ApiControllerBase
        public ApiControllerBase(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        public HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, Func<HttpResponseMessage> func)
        {
            HttpResponseMessage response = null;
            try
            {
                response = func.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }

                LogError(ex);
                response = request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            catch (DbUpdateException db)
            {
                this.LogError(db.InnerException);
                response = request.CreateResponse(HttpStatusCode.BadRequest, db.InnerException.Message);
            }
            catch (Exception ex)
            {
                this.LogError(ex);
                response = request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        public void LogError(Exception ex)
        {
            Error error = new Error()
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };

            _errorBusiness.Add(error);
            _errorBusiness.Save();
        }
    }
}