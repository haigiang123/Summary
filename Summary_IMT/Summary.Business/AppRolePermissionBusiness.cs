using Summary.Data.Repositories;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Business
{
    public interface IAppRolePermissionBusiness
    {
        byte[] GetPermissionByUserId(string userId);
        List<AppRole> GetRoles(string userId);
    }

    public class AppRolePermissionBusiness : IAppRolePermissionBusiness
    {
        private IAppRolePermissionRepository _appRolePermissionRepository;
        private IAppRoleRepository _appRoleRepository;

        public AppRolePermissionBusiness(IAppRolePermissionRepository appRolePermissionRepository, IAppRoleRepository appRoleRepository)
        {
            _appRolePermissionRepository = appRolePermissionRepository;
            _appRoleRepository = appRoleRepository;
        }

        public byte[] GetPermissionByUserId(string userId)
        {
            var a = _appRolePermissionRepository.GetPermission(userId);
            return a;
        }

        public List<AppRole> GetRoles(string userId)
        {
            var a = _appRoleRepository.GetAll();

            return a.ToList();
        }

    }
}
