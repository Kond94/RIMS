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
    public class EggManagementController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EggManagementController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
            var incubator = new Incubator();
            var userId = await GetCurrentUserId();
            if (_context.Incubators.Any())
            {

            if (id.HasValue)
            {
                incubator = _context.Incubators.Include(i => i.IncubatorModel).SingleOrDefault(i => i.Id == id);
            }
            else
            {
                incubator = _context.Incubators.Include(i => i.IncubatorModel).First();

            }

            var racks = _context.Racks.Where(r => r.IncubatorId == incubator.Id).ToList();
            var rackIds = _context.Racks.Where(r => r.IncubatorId == incubator.Id).Select(r => r.Id).ToArray();
            var trays = _context.Trays.Include(rc => rc.Eggtype).Where(rc => rackIds.Contains(rc.RackId)).ToList();

            var viewModel = new EggManagementViewModel
            {
                Incubator = incubator,
                Racks = racks,
                Trays = trays,
                Incubators = await _context.Incubators.Where(i => i.IdentityUserId == userId).ToListAsync(),
                EggTypes = await _context.EggTypes.ToListAsync()


            };
            return View(viewModel);
            }
                else
            {
                var viewModel = new EggManagementViewModel {
                Incubator = new Incubator()
                };

                return View(viewModel);

            }
        }

        public IActionResult ChangeTray(int trayId, int eggTypeId, int incubatorId)
        {
            var tray = _context.Trays.SingleOrDefault(t => t.Id == trayId);

            tray.EggTypeId = eggTypeId;

            tray.DateAdded = DateTime.Today.Date;

            _context.Update(tray);

            _context.SaveChanges();

            tray = _context.Trays.Include(t => t.Eggtype).SingleOrDefault(t => t.Id == trayId);

            return Ok(tray.Eggtype.Name);
        }
    }
}