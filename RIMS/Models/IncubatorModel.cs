using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    [Table("IncubatorModels")]
    public class IncubatorModel
    {
        public int Id { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public byte RackWidth { get; set; }
        [Required]
        public byte RackLength { get; set; }
        [Required]
        public byte RackHeight { get; set; }
    }
}
