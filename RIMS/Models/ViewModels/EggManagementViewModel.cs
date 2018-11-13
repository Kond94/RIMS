using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class EggManagementViewModel
    {
        public EggManagementViewModel()
        {
            Incubators = new Collection<Incubator>();
            Racks = new Collection<Rack>();
            EggTypes = new Collection<EggType>();
            Trays = new Collection<Tray>();

        }
        public Incubator Incubator { get; set; }

        public ICollection<Incubator> Incubators { get; set; }
        public ICollection<Rack> Racks { get; set; }
        public ICollection<EggType> EggTypes { get; set; }
        public ICollection<Tray> Trays { get; set; }
    }
}
