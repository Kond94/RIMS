using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("Measurements")]
    public class Measurement
    {
        [Required]
        public int MeasurementId { get; set; }
        [Required]
        public int IncubatorId { get; set; }
        [Required]
        public int MeasurementTypeId { get; set; }
        [Required]
        public decimal MeasuredValue { get; set; }
        [Required]
        public System.DateTime MeasuredDate { get; set; }
        public virtual Incubator Incubator { get; set; }
        public virtual MeasurementType MeasurementType { get; set; }
    }

}
