using Summary.Business;
using Summary.Model.Models;

using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Summary.API
{
    public class APIControllerBase : ApiController
    {
        private IErrorBusiness _errorBusiness;

        public APIControllerBase(IErrorBusiness errorBusiness)
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

        private void LogError(Exception ex)
        {
            Error error = new Error()
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                CreatedDate = DateTime.Now
            };

            _errorBusiness.Add(error);
            _errorBusiness.Save();
        }

    }
}
