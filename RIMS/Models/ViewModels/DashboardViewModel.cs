using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Incubators = new Collection<Incubator>();
            Measurements = new Collection<Measurement>();
        }
        public Incubator Incubator { get; set; }
        public ICollection<Incubator> Incubators { get; set; }

        public ICollection<Measurement> Measurements { get; set; }

        public DateTime TimeStamp { get; set; }
        public double DataValue { get; set; }
    }
}
