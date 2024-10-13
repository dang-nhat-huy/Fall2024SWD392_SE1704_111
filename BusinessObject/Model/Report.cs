using System;
using System.Collections.Generic;
using static BusinessObject.ReportEnum;

namespace BusinessObject.Model
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public string? ReportName { get; set; }
        public string? ReportLink { get; set; }
        public int? StylistId { get; set; }
        public int? BookingId { get; set; }
        public ReportStatusEnum? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual User? Stylist { get; set; }
    }
}
