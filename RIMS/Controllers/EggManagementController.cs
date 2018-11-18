using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (_context.Incubators.Where(i => i.IdentityUserId == userId).Any())
            {

            if (id.HasValue)
            {
                incubator = _context.Incubators.Include(i => i.IncubatorModel).SingleOrDefault(i => i.Id == id);
            }
            else
            {
                incubator = _context.Incubators.Include(i => i.IncubatorModel).Where(i => i.IdentityUserId == userId).First();
            }

            var racks = _context.Racks.Where(r => r.IncubatorId == incubator.Id).ToList();
            var rackIds = _context.Racks.Where(r => r.IncubatorId == incubator.Id).Select(r => r.Id).ToArray();
            var trays = _context.Trays.Include(rc => rc.Eggtype).Where(rc => rackIds.Contains(rc.RackId)).ToList();
            var candlingDates = _context.Trays.Where(t => t.Rack.IncubatorId == incubator.Id).Where(t => t.EggTypeId != 1).Where(t => t.CandlingDate > DateTime.Now.Date).GroupBy(t => t.CandlingDate).Select(t => t.Key).ToList();
            var hatchPreparationDates = _context.Trays.Where(t => t.Rack.IncubatorId == incubator.Id).Where(t => t.EggTypeId != 1).Where(t => t.CandlingDate < DateTime.Now.Date && t.HatchPreparationDate >= DateTime.Now.Date).GroupBy(t => t.HatchPreparationDate).Select(t => t.Key).ToList();
            var HatchDates = _context.Trays.Where(t => t.Rack.IncubatorId == incubator.Id).Where(t => t.EggTypeId != 1).Where(t => t.HatchPreparationDate < DateTime.Now.Date && t.HatchDate >= DateTime.Now.Date).GroupBy(t => t.HatchDate).Select(t => t.Key).ToList();

            var candlingTrays = new Collection<ActionGroup>();
            var hatchPreparationTrays = new Collection<ActionGroup>();
            var hatchTrays = new Collection<ActionGroup>();

                foreach (var candlingDate in candlingDates)
                {
                    var candlingGroup = new ActionGroup()
                    {
                        TrayIds = _context.Trays.Where(t => t.Rack.IncubatorId == incubator.Id).Where(t => t.EggTypeId != 1).Where(t => t.CandlingDate == candlingDate).Select(t => t.Id).ToArray(),
                        ActionDate = candlingDate
                    };

                    candlingTrays.Add(candlingGroup);
                }

                foreach (var hatchPreparationDate in hatchPreparationDates)
                {
                    var hatchPreparationGroup = new ActionGroup()
                    {
                        TrayIds = _context.Trays.Where(t => t.Rack.IncubatorId == incubator.Id).Where(t => t.EggTypeId != 1).Where(t => t.HatchPreparationDate == hatchPreparationDate).Select(t => t.Id).ToArray(),
                        ActionDate = hatchPreparationDate
                    };

                    hatchPreparationTrays.Add(hatchPreparationGroup);
                }

                foreach (var hatchDate in HatchDates)
                {
                    var hatchGroup = new ActionGroup()
                    {
                        TrayIds = _context.Trays.Where(t => t.Rack.IncubatorId == incubator.Id).Where(t => t.EggTypeId != 1).Where(t => t.HatchDate == hatchDate).Select(t => t.Id).ToArray(),
                        ActionDate = hatchDate
                    };

                    hatchTrays.Add(hatchGroup);
                }
                var viewModel = new EggManagementViewModel
            {
                Incubator = incubator,
                Racks = racks,
                Trays = trays,
                Incubators = await _context.Incubators.Where(i => i.IdentityUserId == userId).ToListAsync(),
                EggTypes = await _context.EggTypes.ToListAsync(),
                CandlingTrays = candlingTrays,
                HatchPreparationTrays = hatchPreparationTrays,
                HatchTrays = hatchTrays
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
            var tray = _context.Trays.Include(t => t.Eggtype).SingleOrDefault(t => t.Id == trayId);

            tray.EggTypeId = eggTypeId;

            _context.Update(tray);
            _context.SaveChanges();


            tray = _context.Trays.Include(t => t.Eggtype).SingleOrDefault(t => t.Id == trayId);


            tray.DateAdded = DateTime.Today.Date;

            tray.CandlingDate = tray.DateAdded.AddDays(tray.Eggtype.CandlingDays);
            tray.HatchPreparationDate = tray.DateAdded.AddDays(tray.Eggtype.HatchPreparationDays);
            tray.HatchDate = tray.DateAdded.AddDays(tray.Eggtype.HatchDays);
            
            _context.Update(tray);

            _context.SaveChanges();

            tray = _context.Trays.Include(t => t.Eggtype).SingleOrDefault(t => t.Id == trayId);

            return Ok(tray.Eggtype.Name);
        }

  
    }
}