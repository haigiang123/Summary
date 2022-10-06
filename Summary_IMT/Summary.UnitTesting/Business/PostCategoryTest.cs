using NUnit.Framework;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Summary.Data.Repositories;
using Summary.Data.Infrastructure;
using Summary.Business;
using Summary.Model.Models;

namespace Summary.UnitTesting.Business
{
    public class PostCategoryTest
    {
        private Mock<IPostCategoryRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IPostCategoryBusiness _categoryBusiness;
        private List<PostCategory> _listCategory;

        [SetUp]
        public void Initialize()
        {
            _mockRepository = new Mock<IPostCategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _categoryBusiness = new PostCategoryBusiness(_mockRepository.Object, _mockUnitOfWork.Object);
            _listCategory = new List<PostCategory>()
            {
                new PostCategory() {ID =1 ,Name="DM1",Status=true },
                new PostCategory() {ID =2 ,Name="DM2",Status=true },
                new PostCategory() {ID =3 ,Name="DM3",Status=true },
            };
        }

        [Test]
        public void PostCategory_Business_GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.GetAll(null)).Returns(_listCategory);

            //call action
            var result = _categoryBusiness.GetAll() as List<PostCategory>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void PostCategory_Business_GetSingleById()
        {
            // setup method
            _mockRepository.Setup(m => m.GetSingleById(1)).Returns(_listCategory.SingleOrDefault(x => x.ID == 1));

            //call action
            var result = _categoryBusiness.GetById(1) as PostCategory;

            //compare
            Assert.IsNotNull(result);
            //Assert.AreEqual(3, result.Count);
        }

    }
}
