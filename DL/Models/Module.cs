using System;
using System.Collections.Generic;

namespace DL.Models
{
    public partial class Module
    {
        public Module()
        {
            Calls = new HashSet<Call>();
        }

        public int Id { get; set; }
        public string ModuleName { get; set; } = null!;

        public virtual ICollection<Call> Calls { get; set; }
    }
}
