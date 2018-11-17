using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("MonitoringDevices")]
    public class MonitoringDevice
    {
        public MonitoringDevice()
        {
            Incubators = new Collection<Incubator>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public virtual ICollection<Incubator> Incubators { get; set; }
    }
}
