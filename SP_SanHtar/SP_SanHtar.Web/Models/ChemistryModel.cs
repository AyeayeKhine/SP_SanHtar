using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
    public class ChemistryModel
    {
        public System.Guid ID { get; set; }
        public long ChemistryID { get; set; }
        public int Chapter { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public string Photo_Path { get; set; }
    }
}
