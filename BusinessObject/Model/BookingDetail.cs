using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Model
{
    public partial class BookingDetail
    {
        public int BookingDetailId { get; set; }
        public int? BookingId { get; set; }
        public int? StylistId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        [JsonIgnore]
        public virtual Booking? Booking { get; set; }
        [JsonIgnore]
        public virtual HairService? Service { get; set; }
        [JsonIgnore]
        public virtual User? Stylist { get; set; }
    }
}
