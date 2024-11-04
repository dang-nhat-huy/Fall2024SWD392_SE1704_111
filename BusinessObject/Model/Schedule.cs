using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class Schedule
    {
        public Schedule()
        {
            BookingDetails = new HashSet<BookingDetail>();
            ScheduleUsers = new HashSet<ScheduleUser>();
        }

        public int ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ScheduleEnum? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<ScheduleUser> ScheduleUsers { get; set; }
    }
}
