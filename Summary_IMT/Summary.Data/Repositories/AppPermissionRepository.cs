using Summary.Data.Infrastructure;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Repositories
{
    public interface IAppPermissionRepository : IRepository<AppPermission>
    {

    }

    public class AppPermissionRepository : Repository<AppPermission>, IAppPermissionRepository
    {
        public AppPermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
