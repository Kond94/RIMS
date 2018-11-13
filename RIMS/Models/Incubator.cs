using Microsoft.AspNetCore.Identity;
using RIMS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models
{
    [Table("Incubators")]
    public class Incubator
    {

        public Incubator()
        {
            Measurements = new Collection<Measurement>();
            Racks = new Collection<Rack>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        [Required]
        [Display(Name ="Capacity (Eggs)")]
        public int IncubatorModelId { get; set; }
        [Display(Name = "Capacity (Eggs)")]
        public virtual IncubatorModel IncubatorModel { get; set; }
        [Required]
        [Display(Name = "Monitoring Device")]
        public int MonitoringDeviceId { get; set; }
        [Display(Name = "Monitoring Device")]
        public virtual MonitoringDevice MonitoringDevice { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<Rack> Racks { get; set; }
    }

}
