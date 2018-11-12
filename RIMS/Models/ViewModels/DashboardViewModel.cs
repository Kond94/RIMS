using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Incubator Incubator { get; set; }
        public ICollection<Incubator> Incubators { get; set; }

        public ICollection<Measurement> Measurements { get; set; }

        public DateTime TimeStamp { get; set; }
        public double DataValue { get; set; }
    }
}
