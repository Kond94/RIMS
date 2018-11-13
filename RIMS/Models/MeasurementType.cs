using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("MeasurementTypes")]
    public class MeasurementType
    {
        public MeasurementType()
        {
            Measurements = new Collection<Measurement>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
    }


    public enum MeasureTypes
    {
        Temperature = 1,
        Humidity = 2
    }
}
