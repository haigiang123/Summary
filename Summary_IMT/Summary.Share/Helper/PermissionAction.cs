using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Share.Helper
{
    public enum PermissionAction : byte
    {
        View = 1,
        Create = 2,
        Edit = 4,
        Delete = 8
    }
}
