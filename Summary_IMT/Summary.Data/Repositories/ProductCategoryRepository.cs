using Summary.Data.Infrastructure;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Repositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        IEnumerable<ProductCategory> GetByAlias(string alias);
    }

    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory DbFactory) : base(DbFactory) { }

        public IEnumerable<ProductCategory> GetByAlias(string alias)
        {
            // return this.GetMulti(x => x.Alias == alias, new string[] { "ProductCategory" });
            return this.DbContext.ProductCategories.ToList();
        }
    } 
}
