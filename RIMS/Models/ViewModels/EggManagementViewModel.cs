using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class EggManagementViewModel
    {
        public Incubator Incubator { get; set; }

        public ICollection<Incubator> Incubators { get; set; }
        public ICollection<Rack> Racks { get; set; }
        public ICollection<EggType> EggTypes { get; set; }
        public ICollection<RackContent> RackContents { get; set; }
    }
}
