using Summary.Data.Infrastructure;
using Summary.Data.Repositories;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Business
{
    public interface IErrorBusiness
    {
        Error Add(Error error);
        void Save();
    }

    public class ErrorBusiness : IErrorBusiness
    {
        IErrorRepository _errorRepository;
        IUnitOfWork _unitOfWork;

        public ErrorBusiness(IUnitOfWork unitOfWork, IErrorRepository errorRepository)
        {
            _unitOfWork = unitOfWork;
            _errorRepository = errorRepository;
        }

        public Error Add(Error error)
        {
            return  _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
