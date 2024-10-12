using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Report
    {
        public Report()
        {
            Bookings = new HashSet<Booking>();
        }

        public int ReportId { get; set; }
        public string ReportName { get; set; } = null!;
        public string ReportLink { get; set; } = null!;
        public int StylistId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual User Stylist { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
