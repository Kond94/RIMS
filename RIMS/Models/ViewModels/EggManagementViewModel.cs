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
            CandlingTrays = new Collection<ActionGroup>();
            HatchPreparationTrays = new Collection<ActionGroup>();
            HatchTrays = new Collection<ActionGroup>();


        }
        public Incubator Incubator { get; set; }

        public ICollection<Incubator> Incubators { get; set; }
        public ICollection<Rack> Racks { get; set; }
        public ICollection<EggType> EggTypes { get; set; }
        public ICollection<Tray> Trays { get; set; }
        public ICollection<ActionGroup> CandlingTrays { get; set; }
        public ICollection<ActionGroup> HatchPreparationTrays { get; set; }
        public ICollection<ActionGroup> HatchTrays { get; set; }

    }
}
