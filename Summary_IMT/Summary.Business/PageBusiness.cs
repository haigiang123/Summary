using Summary.Data.Infrastructure;
using Summary.Data.Repositories;
using Summary_IMT.Summary.Model.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Business
{
    public interface IPageBusiness
    {
        Page GetByAlias(string alias);
    }

    public class PageBusiness : IPageBusiness
    {
        private IUnitOfWork _unitOfWork;
        private IPageRepository _pageRepository;

        public PageBusiness(IUnitOfWork unitOfWork, IPageRepository pageRepository)
        {
            _unitOfWork = unitOfWork;
            _pageRepository = pageRepository;
        }

        public Page GetByAlias(string alias)
        {
            return _pageRepository.GetSingleByCondition(x => x.Alias == alias);
        }
    }
}
