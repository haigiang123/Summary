using Summary.Data.Infrastructure;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Business
{
    public interface IProductCategoryBusiness : IRepository<ProductCategory>
    {
    }

    public class ProductCategoryBusiness : Repository<ProductCategory>, IProductCategoryBusiness
    {
        public ProductCategoryBusiness(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
