using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("Measurements")]
    public class Measurement
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IncubatorId { get; set; }
        public virtual Incubator Incubator { get; set; }
        [Required]
        public int MeasurementTypeId { get; set; }
        public virtual MeasurementType MeasurementType { get; set; }
        [Required]
        public decimal MeasuredValue { get; set; }
        [Required]
        public DateTime MeasuredDate { get; set; }
    }

}
