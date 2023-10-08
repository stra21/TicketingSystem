using System;
using System.Collections.Generic;

namespace DL.Models
{
    public partial class UploadedFile
    {
        public long Id { get; set; }
        public long FkCallId { get; set; }
        public string FilePath { get; set; } = null!;

        public virtual Call FkCall { get; set; } = null!;
    }
}
