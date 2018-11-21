using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    [Table("Racks")]
    public class Rack
    {
        public Rack()
        {
            Trays = new Collection<Tray>();
        }
        public int Id { get; set; }
        public int IncubatorId { get; set; }
        public Incubator Incubator { get; set; }
        [Required]
        public byte RackNumber { get; set; }
        public virtual ICollection<Tray> Trays { get; set; }

    }
}
