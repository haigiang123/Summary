using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Share.Helper
{
    public enum PermissionObject : byte
    {
        Admin = 0,
        User = 1,
        Account = 2,
        Last = Byte.MaxValue - 1
    }
}
