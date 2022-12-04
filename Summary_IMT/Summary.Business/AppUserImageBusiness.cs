using Summary.Data.Infrastructure;
using Summary.Data.Repositories;
using Summary.Model.Models;
using Summary.Share.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Business
{
    public interface IAppUserImageBusiness
    {
        List<AppUserImage> GetAllImage();
        bool InsertImage(List<AppUserImage> appUserImages);
    }

    public class AppUserImageBusiness : IAppUserImageBusiness
    {
        private IAppRoleRepository _appRoleRepository;
        private IAppUserImageRepository _appUserImageRepository;
        private IUnitOfWork _unitOfWork;

        public AppUserImageBusiness(IAppUserImageRepository appUserImageRepository, IUnitOfWork unitOfWork, IAppRoleRepository appRoleRepository)
        {
            _appUserImageRepository = appUserImageRepository;
            _unitOfWork = unitOfWork;
            _appRoleRepository = appRoleRepository;
        }

        public List<AppUserImage> GetAllImage()
        {
            var result = _appUserImageRepository.GetAll().ToList();

            return result;
        }

        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public bool InsertImage(List<AppUserImage> appUserImages)
        {
            try
            {
                foreach (var item in appUserImages)
                {
                    _appUserImageRepository.Add(item);
                }

                this.SaveChange();

                return true;
            }
            catch
            {
                return false;
            } 
        }

    }
}
