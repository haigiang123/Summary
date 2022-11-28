using Summary.Data.Infrastructure;
using Summary.Model.Models;
using Summary.Share.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Repositories
{
    public interface IAppRolePermissionRepository : IRepository<AppRolePermission>
    {
        byte[] GetPermission(string userId);
    }

    public class AppRolePermissionRepository : Repository<AppRolePermission>, IAppRolePermissionRepository
    {
        public AppRolePermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public byte[] GetPermission(string userId)
        {
            byte[] result = new byte[(byte)PermissionObject.Last];

            var a = (from ur in DbContext.AppUserRoles
                   join r in DbContext.AppRoles
                     on ur.RoleId equals r.Id
                   join rp in DbContext.AppRolePermissions
                     on r.Id equals rp.RoleID
                   join p in DbContext.AppPermissions
                     on rp.PermissionId equals p.Id
                   where ur.UserId == userId
                   select new PermissionFake
                   {
                       RoleId = r.ManualId,
                       PermissionId = p.ManualId
                   }
                   ).ToList();

            foreach(var item in a)
            {
                result[item.RoleId.Value] |= item.PermissionId.Value;
            }

            return result;
        }

        private class PermissionFake
        {
            public byte? RoleId { get; set; }
            public byte? PermissionId { get; set; }
        } 
    }
}
