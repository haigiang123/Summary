using Summary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data.Infrastructure
{
    public interface IDbFactory: IDisposable
    {
        SummaryDbContext Init();
    }
}
