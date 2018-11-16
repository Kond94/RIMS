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
            TempretureMeasurements = new decimal[] { };
            HumidityMeasurements = new decimal[] { };
        }
        public Incubator Incubator { get; set; }
        public ICollection<Incubator> Incubators { get; set; }


        public RecentMeasurements RecentMeasurements { get; set; }

        public DateTime TimeStamp { get; set; }
        public double DataValue { get; set; }

        public decimal[] TempretureMeasurements { get; set; }
        public decimal[] HumidityMeasurements { get; set; }


    }
}
