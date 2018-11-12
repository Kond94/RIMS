using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    [Table("RackContents")]
    public class RackContent
    {
        public int RackContentId { get; set; }

        public int RackId { get; set; }
        public Rack Rack { get; set; }
        public byte Row { get; set; }
        public byte Column { get; set; }
        public DateTime DateAdded { get; set; }
        public int EggTypeId { get; set; }
        public EggType Eggtype { get; set; }
    }
}
