using System;
using System.Collections.Generic;

namespace DL.Models
{
    public partial class CallType
    {
        public CallType()
        {
            Calls = new HashSet<Call>();
        }

        public short Id { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<Call> Calls { get; set; }
    }
}
