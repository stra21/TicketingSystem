using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models
{
    public class CallsDTO:Call
    {
        public List<UploadedFile> Attachments { get; set; }
    }
}
