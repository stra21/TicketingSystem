using System;
using System.Collections.Generic;

namespace DL.Models
{
    public partial class Call
    {
        public Call()
        {
            Comments = new HashSet<Comment>();
            InverseFkRefCallNavigation = new HashSet<Call>();
            UploadedFiles = new HashSet<UploadedFile>();
        }

        public long Id { get; set; }
        public int? FkAssignedTo { get; set; }
        public short FkCallType { get; set; }
        public int FkModule { get; set; }
        public string FormIdentifier { get; set; } = null!;
        public long? FkRefCall { get; set; }
        public int Priority { get; set; }
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Suggestion { get; set; }
        public int SupportStatus { get; set; }
        public int UserStatus { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int? FkClosedBy { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int FkCreatedBy { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public int? FkModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDateTime { get; set; }
        public int? FkDeletedBy { get; set; }

        public virtual User? FkAssignedToNavigation { get; set; }
        public virtual CallType? FkCallTypeNavigation { get; set; } = null!;
        public virtual User? FkCreatedByNavigation { get; set; } = null!;
        public virtual User? FkDeletedByNavigation { get; set; }
        public virtual User? FkModifiedByNavigation { get; set; }
        public virtual Module? FkModuleNavigation { get; set; } = null!;
        public virtual Call? FkRefCallNavigation { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Call> InverseFkRefCallNavigation { get; set; }
        public virtual ICollection<UploadedFile> UploadedFiles { get; set; }
    }
}
