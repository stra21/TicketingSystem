using System;
using System.Collections.Generic;

namespace DL.Models
{
    public partial class Comment
    {
        public long Id { get; set; }
        public string Comment1 { get; set; } = null!;
        public int FkCommenterId { get; set; }
        public long FkCallId { get; set; }

        public virtual Call FkCall { get; set; } = null!;
        public virtual User FkCommenter { get; set; } = null!;
    }
}
