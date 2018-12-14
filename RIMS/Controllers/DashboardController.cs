using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models;
using RIMS.Models.ViewModels;

namespace RIMS.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            IdentityUser usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index(int? incubatorId, string graphTimeFrame)
        {
            if (graphTimeFrame == null)
            { graphTimeFrame = "Hour"; }

            var userId = await GetCurrentUserId();

            if (_context.Incubators.Where(i => i.IdentityUserId == userId).Any())
            {

                var incubators = await _context.Incubators.Where(i => i.IdentityUserId == userId).ToListAsync();



                if (!incubatorId.HasValue)
                {
                    var incubator = incubators.First();
                    

                    
                       var measurements = GetMeasurements(graphTimeFrame, incubator.Id);
                        
                        var viewModel = new DashboardViewModel
                        {
                            Incubator = incubator,
                            Incubators = incubators,
                            TempretureMeasurements = measurements.TempretureMeasurements,
                            HumidityMeasurements = measurements.HumidityMeasurements,
                            RecentMeasurements = MostRecentMeasurements(incubator.Id),
                            Labels = measurements.Labels,
                            TimeFrame = measurements.TimeFrame
                        };
                        return View(viewModel);
                    
                    
                }
                else
                {
                    var incubator = incubators.SingleOrDefault(i => i.Id == incubatorId);

                    
                        var measurements = GetMeasurements(graphTimeFrame, incubator.Id);
                       
                        var viewModel = new DashboardViewModel
                        {
                            Incubator = incubator,
                            Incubators = incubators,
                            TempretureMeasurements = measurements.TempretureMeasurements,
                            HumidityMeasurements = measurements.HumidityMeasurements,
                            RecentMeasurements = MostRecentMeasurements(incubator.Id),
                            Labels = measurements.Labels
                        };
                        return View(viewModel);
                    
                    
                }
            }
            else
            {
                var viewModel = new DashboardViewModel
                {
                    Incubator = new Incubator()
                };

                return View(viewModel);

            }
        }


        public RecentMeasurements MostRecentMeasurements(int inubatorId)
        {
            var recent = new RecentMeasurements();

            var last3 = _context.Measurements
                .Where(m => m.IncubatorId == inubatorId)
                .Select(m => m).Include(l => l.MeasurementType).Distinct().
                OrderByDescending(m => m.MeasuredDate).Take(3).ToList();

            if (last3.Any())
            {
                var temp = last3.FirstOrDefault(m => m.MeasurementTypeId == 1);
                var humd = last3.FirstOrDefault(m => m.MeasurementTypeId == 2);

                if (temp != null)
                {
                    recent.MeasuredDate = temp.MeasuredDate;
                    recent.Temperature = temp.MeasuredValue;
                }

                if (humd != null) { recent.Humidity = humd.MeasuredValue; }
            }

            return recent;
        }
        [EnableCors("AllowAllOrigins")]
        [AllowAnonymous]
        public ActionResult PostData(int id, decimal? temp, decimal? light)
        {
            var results = "Success";
            var reported = DateTime.Now;

            try
            {
                var monitoringDevice = _context.MonitoringDevices.SingleOrDefault(mD => mD.Id == id);

                if (monitoringDevice == null)
                {
                    results = "Unknown device";
                }
                else
                {
                    

                    if (temp.HasValue)
                    {
                        // add temperature
                        _context.Measurements.Add(new Measurement
                        {
                            MeasurementTypeId = (int)MeasurementTypesEnum.Temperature,
                            IncubatorId = _context.Incubators.SingleOrDefault(i => i.MonitoringDeviceId == id).Id,
                            MeasuredValue = temp.Value,
                            MeasuredDate = reported
                        });
                    }

                    if (light.HasValue)
                    {
                        // add temperature
                        _context.Measurements.Add(new Measurement
                        {
                            MeasurementTypeId = (int)MeasurementTypesEnum.Humidity,
                            IncubatorId = _context.Incubators.SingleOrDefault(i => i.MonitoringDeviceId == id).Id,
                            MeasuredValue = light.Value,
                            MeasuredDate = reported
                        });
                    }

                    // save it all
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                results = "Exception: " + ex.Message;
            }

            return Content(results);
        }

        public GraphMeasurements GetMeasurements(string graphTimeFrame, int incubatorId)
        {
            var temperatureMeasurements = new decimal[7];
            var humidityMeasurements = new decimal[7];
            var measurements = new GraphMeasurements();

            switch (graphTimeFrame)
            {
                case "Month":
                    temperatureMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-25) && m.MeasuredDate > DateTime.Now.AddDays(-30)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-20) && m.MeasuredDate > DateTime.Now.AddDays(-25)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-15) && m.MeasuredDate > DateTime.Now.AddDays(-20)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-10) && m.MeasuredDate > DateTime.Now.AddDays(-15)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-5) && m.MeasuredDate > DateTime.Now.AddDays(-10)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    humidityMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-25) && m.MeasuredDate > DateTime.Now.AddDays(-30)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-20) && m.MeasuredDate > DateTime.Now.AddDays(-25)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-15) && m.MeasuredDate > DateTime.Now.AddDays(-20)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-10) && m.MeasuredDate > DateTime.Now.AddDays(-15)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-5) && m.MeasuredDate > DateTime.Now.AddDays(-10)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    measurements.TempretureMeasurements = temperatureMeasurements;
                    measurements. HumidityMeasurements = humidityMeasurements;

                    measurements.Labels = new string[6] {
                    DateTime.Now.AddDays(-25).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-20).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-15).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-10).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-5).ToString("dd-MMM"),
                    DateTime.Now.ToString("dd-MMM-yyyy") };

                    measurements.TimeFrame = "Month";
            
            break;

                case "Week":
                    temperatureMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-5) && m.MeasuredDate > DateTime.Now.AddDays(-6)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-4) && m.MeasuredDate > DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-3) && m.MeasuredDate > DateTime.Now.AddDays(-4)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-2) && m.MeasuredDate > DateTime.Now.AddDays(-3)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-1) && m.MeasuredDate > DateTime.Now.AddDays(-2)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddDays(-1)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    humidityMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-5) && m.MeasuredDate > DateTime.Now.AddDays(-6)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-4) && m.MeasuredDate > DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-3) && m.MeasuredDate > DateTime.Now.AddDays(-4)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-2) && m.MeasuredDate > DateTime.Now.AddDays(-3)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-1) && m.MeasuredDate > DateTime.Now.AddDays(-2)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddDays(-1)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    measurements.TempretureMeasurements = temperatureMeasurements;
                    measurements.HumidityMeasurements = humidityMeasurements;

                    measurements.Labels = new string[6] {
                    DateTime.Now.AddDays(-5).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-4).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-3).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-2).ToString("dd-MMM"),
                    DateTime.Now.AddDays(-1).ToString("dd-MMM"),
                    DateTime.Now.ToString("dd-MMM") };

                    measurements.TimeFrame = "Week";

                    break;

                case "Day":
                    temperatureMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-20) && m.MeasuredDate > DateTime.Now.AddHours(-24)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-16) && m.MeasuredDate > DateTime.Now.AddHours(-20)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-12) && m.MeasuredDate > DateTime.Now.AddHours(-16)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-8) && m.MeasuredDate > DateTime.Now.AddHours(-12)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-4) && m.MeasuredDate > DateTime.Now.AddHours(-8)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddHours(-4)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    humidityMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-20) && m.MeasuredDate > DateTime.Now.AddHours(-24)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-16) && m.MeasuredDate > DateTime.Now.AddHours(-20)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-12) && m.MeasuredDate > DateTime.Now.AddHours(-16)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-8) && m.MeasuredDate > DateTime.Now.AddHours(-12)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddHours(-4) && m.MeasuredDate > DateTime.Now.AddHours(-8)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddHours(-4)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    measurements.TempretureMeasurements = temperatureMeasurements;
                    measurements.HumidityMeasurements = humidityMeasurements;

                    measurements.Labels = new string[6] {
                    DateTime.Now.AddHours(-28).ToString("dd-MMM HH:mm"),
                    DateTime.Now.AddHours(-24).ToString("dd-MMM HH:mm"),
                    DateTime.Now.AddHours(-18).ToString("dd-MMM HH:mm"),
                    DateTime.Now.AddHours(-12).ToString("dd-MMM HH:mm"),
                    DateTime.Now.AddHours(-6).ToString("dd-MMM HH:mm"),
                    DateTime.Now.ToString("dd-MMM HH:mm") };

                    measurements.TimeFrame = "Day";

                    break;

                case "Hour":
                    temperatureMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-50) && m.MeasuredDate > DateTime.Now.AddMinutes(-60)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-40) && m.MeasuredDate > DateTime.Now.AddMinutes(-50)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-30) && m.MeasuredDate > DateTime.Now.AddMinutes(-40)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-20) && m.MeasuredDate > DateTime.Now.AddMinutes(-30)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-10) && m.MeasuredDate > DateTime.Now.AddMinutes(-20)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddMinutes(-10)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    humidityMeasurements = new decimal[6] {
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-50) && m.MeasuredDate > DateTime.Now.AddMinutes(-60)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-40) && m.MeasuredDate > DateTime.Now.AddMinutes(-50)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-30) && m.MeasuredDate > DateTime.Now.AddMinutes(-40)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-20) && m.MeasuredDate > DateTime.Now.AddMinutes(-30)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddMinutes(-10) && m.MeasuredDate > DateTime.Now.AddMinutes(-20)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average(),
                    _context.Measurements.Where(m => m.IncubatorId == incubatorId).Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now && m.MeasuredDate > DateTime.Now.AddMinutes(-10)).Select(m => m.MeasuredValue).DefaultIfEmpty().Average()
                };
                    measurements.TempretureMeasurements = temperatureMeasurements;
                    measurements.HumidityMeasurements = humidityMeasurements;

                    measurements.Labels = new string[6] {
                    DateTime.Now.AddMinutes(-50).ToString("HH:mm"),
                    DateTime.Now.AddMinutes(-40).ToString("HH:mm"),
                    DateTime.Now.AddMinutes(-30).ToString("HH:mm"),
                    DateTime.Now.AddMinutes(-20).ToString("HH:mm"),
                    DateTime.Now.AddMinutes(-10).ToString("HH:mm"),
                    DateTime.Now.ToString("HH:mm") };

                    measurements.TimeFrame = "Hour";

                    break;

                default:
                    break;
            }
            return measurements; 
        }
    }
}
