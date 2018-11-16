using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class RecentMeasurements
    {
        public DateTime? MeasuredDate { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
    }
}
