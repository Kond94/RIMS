using Microsoft.AspNetCore.Identity;
using RIMS.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("Incubators")]
    public class Incubator
    {
        [Required]
        public int IncubatorId { get; set; }
        [Required]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        [Required]
        [Display(Name ="Capacity (Eggs)")]
        public int IncubatorModelId { get; set; }
        [Required]
        [Display(Name = "Monitoring Device")]
        public int MonitoringDeviceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string LastIpAddress { get; set; }
        public virtual MonitoringDevice MonitoringDevice { get; set; }
        public virtual IncubatorModel IncubatorModel { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<Rack> Racks { get; set; }
    }

}
