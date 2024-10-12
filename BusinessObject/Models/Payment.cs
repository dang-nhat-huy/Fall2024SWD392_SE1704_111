using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public double? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? Status { get; set; }
        public int? BookingId { get; set; }
        public int? PaymentTypeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual Booking Booking { get; set; } = null!;
        public virtual PaymentType PaymentType { get; set; } = null!;
    }
}
