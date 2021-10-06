using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
    public class ChemistryDetailModel
    {
        public System.Guid ID { get; set; }
        public long ChemistryDetailID { get; set; }
        public System.Guid ChemistryID { get; set; }
        public int Part { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public string Video_Path { get; set; }
        public int Chapter { get; set; }
    }
}
