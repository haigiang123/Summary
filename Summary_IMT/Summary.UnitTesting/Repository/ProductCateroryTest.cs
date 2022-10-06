using NUnit.Framework;

using Summary.Data.Infrastructure;
using Summary.Data.Repositories;
using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.UnitTesting.Repository
{
    [TestFixture]
    class ProductCateroryTest
    {
        IDbFactory _factory;
        IProductCategoryRepository _productCategoryRepository;
        IUnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _factory = new DbFactory();
            _productCategoryRepository = new ProductCategoryRepository(_factory);
            _unitOfWork = new UnitOfWork(_factory);
        }

        [TestCase("ao-nam")]
        public void GetByAlias(string alias)
        {
            var result = _productCategoryRepository.GetMulti(x => x.Alias == alias);
            Assert.IsTrue(result.ToList().Count == 1, "fail");
        }

        [Test()]
        public void Add(ProductCategory productCategory)
        {
            productCategory = new ProductCategory
            {
                Alias = "alias",
                Name = "alias",
                Status = true
            };

            var result = _productCategoryRepository.Add(productCategory);
            _unitOfWork.Commit();

            Assert.IsNotNull(result, "fail");
            Assert.AreEqual(3, result.ID, "fail");
        }

    }
}
