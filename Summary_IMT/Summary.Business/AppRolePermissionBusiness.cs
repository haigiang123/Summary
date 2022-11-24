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
    }

    public class AppRolePermissionBusiness : IAppRolePermissionBusiness
    {
        private IAppRolePermissionRepository _appRolePermissionRepository;

        public AppRolePermissionBusiness(IAppRolePermissionRepository appRolePermissionRepository)
        {
            _appRolePermissionRepository = appRolePermissionRepository;
        }

        public byte[] GetPermissionByUserId(string userId)
        {
            var a = _appRolePermissionRepository.GetPermission(userId);
            return a;
        }

    }
}
