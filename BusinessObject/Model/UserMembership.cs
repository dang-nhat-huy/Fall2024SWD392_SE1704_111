using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class UserMembership
    {
        public int UserMembershipId { get; set; }
        public int? UserProfileId { get; set; }
        public int? MembershipId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual Membership? Membership { get; set; }
        public virtual UserProfile? UserProfile { get; set; }
    }
}
