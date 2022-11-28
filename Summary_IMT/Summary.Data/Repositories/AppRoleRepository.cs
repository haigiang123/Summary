using Summary.Data.Infrastructure;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Repositories
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {

    }

    public class AppRoleRepository : Repository<AppRole>, IAppRoleRepository
    {
        public AppRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
