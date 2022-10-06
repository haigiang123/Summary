using NUnit.Framework;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Summary.Business;
using Summary.Data.Repositories;
using Summary.Data.Infrastructure;
using Summary.Model.Models;

namespace Summary.UnitTesting.Business
{
    [TestFixture]
    public class PostTest
    {
        Mock<IPostRepository> _postRepository;
        Mock<IUnitOfWork> _unitOfWork;

        PostBusiness _sut;
        List<Post> _posts;

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IPostRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _sut = new PostBusiness(_unitOfWork.Object, _postRepository.Object);

            _posts = new List<Post>
            {
                new Post() {ID =1 ,Name="DM1",Status=true },
                new Post() {ID =2 ,Name="DM2",Status=true },
                new Post() {ID =3 ,Name="DM3",Status=true },
            };
        }

        [Test]
        public void GetAll()
        {
            _postRepository.Setup(x => x.GetAll(null)).Returns(_posts);

            var result = _sut.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count(), "fail");
        }

    }
}
