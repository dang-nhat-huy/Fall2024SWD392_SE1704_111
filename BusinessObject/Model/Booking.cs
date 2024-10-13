using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
            Payments = new HashSet<Payment>();
            Reports = new HashSet<Report>();
        }

        public int BookingId { get; set; }
        public double? TotalPrice { get; set; }
        public int? VoucherId { get; set; }
        public int? ManagerId { get; set; }
        public int? CustomerId { get; set; }
        public int? StaffId { get; set; }
        public int? ScheduleId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual User? Customer { get; set; }
        public virtual User? Manager { get; set; }
        public virtual Schedule? Schedule { get; set; }
        public virtual User? Staff { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
