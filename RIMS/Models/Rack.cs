using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    [Table("Racks")]
    public class Rack
    {
        public int RackId { get; set; }
        public int IncubatorId { get; set; }
        public byte RackNumber { get; set; }
        public virtual ICollection<RackContent> RackContents { get; set; }

    }
}
