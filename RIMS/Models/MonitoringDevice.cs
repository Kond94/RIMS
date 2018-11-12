using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("MonitoringDevices")]
    public class MonitoringDevice
    {
        [Required]
        public int MonitoringDeviceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Incubator> Incubators { get; set; }
    }
}
