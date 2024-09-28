using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class ScheduleUser
    {
        public int ScheduleUserId { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual Schedule Schedule { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
