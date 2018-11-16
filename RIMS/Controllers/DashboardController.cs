using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models;
using RIMS.Models.ViewModels;

namespace RIMS.Controllers
{
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

        public async Task<IActionResult> Index(int? id)
        {
            if (_context.Incubators.Any())
            {

                var userId = await GetCurrentUserId();
                var incubators = await _context.Incubators.Where(i => i.IdentityUserId == userId).ToListAsync();



                if (!id.HasValue)
                {
                    var incubator = incubators.First();
                    var measurements = _context.Measurements.Take(3).Where(m => m.IncubatorId == incubator.Id);

                    if (measurements.Any())
                    {
                        var temperatureMeasurements = new decimal[] {
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-35)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-25)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-25) && m.MeasuredDate < DateTime.Now.AddDays(-20)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-20) && m.MeasuredDate < DateTime.Now.AddDays(-15)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-15) && m.MeasuredDate < DateTime.Now.AddDays(-10)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-10) && m.MeasuredDate < DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-5) && m.MeasuredDate <= DateTime.Now).Select(m => m.MeasuredValue).Average()
                };
                        var humidityMeasurements = new decimal[] {
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-35)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-25)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-25) && m.MeasuredDate < DateTime.Now.AddDays(-20)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-20) && m.MeasuredDate < DateTime.Now.AddDays(-15)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-15) && m.MeasuredDate < DateTime.Now.AddDays(-10)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-10) && m.MeasuredDate < DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-5) && m.MeasuredDate <= DateTime.Now).Select(m => m.MeasuredValue).Average()
                };
                        var viewModel = new DashboardViewModel
                        {
                            Incubator = incubator,
                            Incubators = incubators,
                            TempretureMeasurements = temperatureMeasurements,
                            HumidityMeasurements = humidityMeasurements,
                            RecentMeasurements = MostRecentMeasurements(incubator.Id)
                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var temperatureMeasurements = new decimal[] { 0, 0, 0, 0, 0, 0, 0 };
                        var humidityMeasurements = new decimal[]  { 0, 0, 0, 0, 0, 0, 0 };
                        var viewModel = new DashboardViewModel {
                            Incubator = incubator,
                            Incubators = incubators,
                            HumidityMeasurements = humidityMeasurements,
                            TempretureMeasurements = temperatureMeasurements,
                            RecentMeasurements = MostRecentMeasurements(incubator.Id)
                        };
                        return View(viewModel);
                    }
                }
                else
                {
                    var incubator = incubators.SingleOrDefault(i => i.Id == id);
                    var measurements = _context.Measurements.Take(3).Where(m => m.IncubatorId == incubator.Id);

                    if (measurements.Any())
                    {
                        var temperatureMeasurements = new decimal[] {
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-35)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-25)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-25) && m.MeasuredDate < DateTime.Now.AddDays(-20)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-20) && m.MeasuredDate < DateTime.Now.AddDays(-15)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-15) && m.MeasuredDate < DateTime.Now.AddDays(-10)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-10) && m.MeasuredDate < DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 1).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-5) && m.MeasuredDate <= DateTime.Now).Select(m => m.MeasuredValue).Average()
                };
                        var humidityMeasurements = new decimal[] {
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate <= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-35)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-30) && m.MeasuredDate < DateTime.Now.AddDays(-25)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-25) && m.MeasuredDate < DateTime.Now.AddDays(-20)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-20) && m.MeasuredDate < DateTime.Now.AddDays(-15)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-15) && m.MeasuredDate < DateTime.Now.AddDays(-10)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-10) && m.MeasuredDate < DateTime.Now.AddDays(-5)).Select(m => m.MeasuredValue).Average(),
                    _context.Measurements.Where(m => m.MeasurementTypeId == 2).Where(m => m.MeasuredDate >= DateTime.Now.AddDays(-5) && m.MeasuredDate <= DateTime.Now).Select(m => m.MeasuredValue).Average()
                };
                        var viewModel = new DashboardViewModel
                        {
                            Incubator = incubator,
                            Incubators = incubators,
                            TempretureMeasurements = temperatureMeasurements,
                            HumidityMeasurements = humidityMeasurements,
                            RecentMeasurements = MostRecentMeasurements(incubator.Id)
                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var temperatureMeasurements = new decimal[] { 0, 0, 0, 0, 0, 0, 0 };
                        var humidityMeasurements = new decimal[] { 0, 0, 0, 0, 0, 0, 0 };
                        var viewModel = new DashboardViewModel
                        {
                            Incubator = incubator,
                            Incubators = incubators,
                            HumidityMeasurements = humidityMeasurements,
                            TempretureMeasurements = temperatureMeasurements,
                            RecentMeasurements = MostRecentMeasurements(incubator.Id)
                        };
                        return View(viewModel);
                    }
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

        public JsonResult GetRealTimeData(int? id)
        {
            Random rdn = new Random();
            var data = new DashboardViewModel
            {
                TimeStamp = DateTime.Now,
                DataValue = rdn.Next(0, 11)
            };

            return Json(data);
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

        public ActionResult PostData(string id, decimal? temp, decimal? light)
        {
            var results = "Success";
            var reported = DateTime.Now;

            try
            {
                var monitoringDevice = _context.MonitoringDevices.SingleOrDefault(mD => mD.Indentifier == id);

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
                            IncubatorId = 13,
                            MeasuredValue = temp.Value,
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
    }
}
