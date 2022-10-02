using Summary.Data.Infrastructure;
using Summary_IMT.Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Repositories
{
    public interface IPageRepository : IRepository<Page>
    {

    }

    public class PageRepository : Repository<Page>, IPageRepository
    {
        public PageRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
