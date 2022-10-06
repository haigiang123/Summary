using Summary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbFactory _dbFactory;
        private SummaryDbContext _dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        } 

        public SummaryDbContext dbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
