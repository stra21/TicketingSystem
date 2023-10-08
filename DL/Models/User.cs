using System;
using System.Collections.Generic;

namespace DL.Models
{
    public partial class User
    {
        public User()
        {
            CallFkAssignedToNavigations = new HashSet<Call>();
            CallFkCreatedByNavigations = new HashSet<Call>();
            CallFkDeletedByNavigations = new HashSet<Call>();
            CallFkModifiedByNavigations = new HashSet<Call>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int UserType { get; set; }

        public virtual ICollection<Call> CallFkAssignedToNavigations { get; set; }
        public virtual ICollection<Call> CallFkCreatedByNavigations { get; set; }
        public virtual ICollection<Call> CallFkDeletedByNavigations { get; set; }
        public virtual ICollection<Call> CallFkModifiedByNavigations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
