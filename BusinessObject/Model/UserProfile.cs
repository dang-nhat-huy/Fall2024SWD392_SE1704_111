using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            UserMemberships = new HashSet<UserMembership>();
        }

        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public string? ImageLink { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
