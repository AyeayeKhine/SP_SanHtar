using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
    public class CryptographyClass
    {
        public string Text { get; set; }
        public byte[] SALT { get; set; }
        public byte[] PlainText { get; set; }
    }
}
