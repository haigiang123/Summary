using Summary.Data.Infrastructure;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Repositories
{
    public interface IAppUserImageRepository : IRepository<AppUserImage>
    {
        
    }

    public class AppUserImageRepository : Repository<AppUserImage>, IAppUserImageRepository
    {
        public AppUserImageRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
