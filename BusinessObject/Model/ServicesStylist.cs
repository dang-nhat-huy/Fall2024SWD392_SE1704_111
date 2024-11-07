using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class ServicesStylist
    {
        public int ServiceStylistId { get; set; }
        public int? StylistId { get; set; }
        public int? ServiceId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual HairService? Service { get; set; }
        public virtual User? Stylist { get; set; }
    }
}
