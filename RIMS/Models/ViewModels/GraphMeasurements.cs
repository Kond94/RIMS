using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class GraphMeasurements
    {
        
        public decimal[] TempretureMeasurements { get; set; }
        public decimal[] HumidityMeasurements { get; set; }

        public string[] Labels { get; set; }

        public string TimeFrame { get; set; }
    }
}
