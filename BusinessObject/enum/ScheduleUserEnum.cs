using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public enum ScheduleUserEnum
    {
        None = 0,
        InQueue,
        Accepted,
        InProgress,
        Delay,
        Complete,
        Cancel,
        Assign,
        NotAssign,
    }

    public enum CreateScheduleUserEnum
    {
        None = 0,
        Assign,
        NotAssign,
    }
}
