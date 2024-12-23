﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static BusinessObject.VoucherEnum;

namespace BusinessObject.Model
{
    public partial class Voucher
    {
        public Voucher()
        {
            Bookings = new HashSet<Booking>();
        }

        public int VoucherId { get; set; }
        public double? DiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public VoucherStatusEnum? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
