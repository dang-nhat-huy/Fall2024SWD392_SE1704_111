using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public enum BookingStatus
    {
        None,
        InQueue,
        InProgress,
        Delay,
        Complete,
        Cancel,
    }
}
