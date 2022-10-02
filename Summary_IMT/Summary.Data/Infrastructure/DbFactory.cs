using Summary_IMT.Summary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private SummaryDbContext dbContext;

        public SummaryDbContext Init()
        {
            return dbContext ?? (dbContext = new SummaryDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }

    }
}
