using System;
using System.Collections.Generic;

namespace BusinessObject.Model
{
    public partial class Membership
    {
        public Membership()
        {
            UserMemberships = new HashSet<UserMembership>();
        }

        public int MembershipId { get; set; }
        public int? Level { get; set; }
        public string? Detail { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
