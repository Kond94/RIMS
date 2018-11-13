using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RIMS.Data;
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
            var userId = await GetCurrentUserId();
            var incubators = await _context.Incubators.Where(i => i.IdentityUserId == userId).ToListAsync();



            if (!id.HasValue)
            {
                var incubator = incubators.First();
                var measurements = await _context.Measurements.Where(m => m.IncubatorId == incubator.Id).ToListAsync();
                var viewModel = new DashboardViewModel
                {
                    Incubator = incubator,
                    Incubators = incubators,
                    Measurements = measurements
                };
                return View(viewModel);

            }
            else
            {
                var incubator = incubators.SingleOrDefault(i => i.Id == id);
                var measurements = await _context.Measurements.Where(m => m.IncubatorId == incubator.Id).ToListAsync();
                var viewModel = new DashboardViewModel
                {
                    Incubator = incubator,
                    Incubators = incubators,
                    Measurements = measurements
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
    }
}
