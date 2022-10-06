using Summary.Data.Infrastructure;
using Summary.Model.Models;

namespace Summary.Data.Repositories
{
    public interface IMenuRepository : IRepository<Function>
    {
    }

    public class MenuRepository : Repository<Function>, IMenuRepository
    {
        public MenuRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}