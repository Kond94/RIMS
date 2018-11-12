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

            if (id.HasValue)
            {
                incubator = _context.Incubators.Include(i => i.IncubatorModel).SingleOrDefault(i => i.IncubatorId == id);
            }
            else
            {
                incubator = _context.Incubators.Include(i => i.IncubatorModel).First();

            }

            var racks = _context.Racks.Where(r => r.IncubatorId == incubator.IncubatorId).ToList();
            var rackIds = _context.Racks.Where(r => r.IncubatorId == incubator.IncubatorId).Select(r => r.RackId).ToArray();
            var rackContents = _context.RackContents.Include(rc => rc.Eggtype).Where(rc => rackIds.Contains(rc.RackId)).ToList();

            var viewModel = new EggManagementViewModel
            {
                Incubator = incubator,
                Racks = racks,
                RackContents = rackContents,
                Incubators = await _context.Incubators.Where(i => i.IdentityUserId == userId).ToListAsync(),
                EggTypes = await _context.EggTypes.ToListAsync()


            };
            return View(viewModel);
        }

        public IActionResult ChangeTray(int trayId, int eggTypeId, int incubatorId)
        {
            var tray = _context.RackContents.SingleOrDefault(t => t.RackContentId == trayId);

            tray.EggTypeId = eggTypeId;

            tray.DateAdded = DateTime.Today.Date;

            _context.Update(tray);

            _context.SaveChanges();

            return RedirectToAction("index",incubatorId);
        }
    }
}