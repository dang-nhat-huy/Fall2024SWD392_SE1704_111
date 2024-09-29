using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class User
    {
        public User()
        {
            BookingCustomers = new HashSet<Booking>();
            BookingDetails = new HashSet<BookingDetail>();
            BookingManagers = new HashSet<Booking>();
            BookingStaffs = new HashSet<Booking>();
            Feedbacks = new HashSet<Feedback>();
            Reports = new HashSet<Report>();
            ScheduleUsers = new HashSet<ScheduleUser>();
            ServicesStylists = new HashSet<ServicesStylist>();
            UserProfiles = new HashSet<UserProfile>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<Booking> BookingCustomers { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<Booking> BookingManagers { get; set; }
        public virtual ICollection<Booking> BookingStaffs { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ScheduleUser> ScheduleUsers { get; set; }
        public virtual ICollection<ServicesStylist> ServicesStylists { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
