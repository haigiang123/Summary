using Summary.Data.Infrastructure;
using Summary_IMT.Summary.Model.Models;

namespace TeduShop.Data.Repositories
{
    public interface IColorRepository : IRepository<Color>
    {
    }

    public class ColorRepository : Repository<Color>, IColorRepository
    {
        public ColorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}