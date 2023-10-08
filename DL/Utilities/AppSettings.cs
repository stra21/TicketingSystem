using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Utilities
{
    public static class AppSettings
    {
        public static string ConnectionString { get; set; }
        public const string DefaultPath = "Content/Uploads/";
        public static string[] AcceptedImageTypes = { "png", "jpeg", "jpg" };
    }
}
