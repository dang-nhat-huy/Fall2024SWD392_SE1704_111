using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public enum User
    {
        None = 0,
        Customer,
        Stylist,
        Staff,
        Manager,
        Admin,
    }

    public enum UserStatus
    {
        None = 0,
        Active,
        Banned,
    }
}
