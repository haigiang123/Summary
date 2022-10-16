using Summary.Business;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Summary.WebApi.Infrastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        private IErrorBusiness _errorBusiness;

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