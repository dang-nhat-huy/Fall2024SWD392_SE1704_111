using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Model
{
    public partial class HairService
    {
        public HairService()
        {
            BookingDetails = new HashSet<BookingDetail>();
            ServicesStylists = new HashSet<ServicesStylist>();
        }

        public int ServiceId { get; set; }
        public string? ImageLink { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public TimeSpan? EstimateTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        [JsonIgnore]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<ServicesStylist> ServicesStylists { get; set; }
    }
}
