using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models;

namespace RIMS.Controllers
{
    [Authorize]
    public class IncubatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IncubatorsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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


        // GET: Incubators
        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserId();
            var applicationDbContext = _context.Incubators.Include(i => i.IncubatorModel).Include(i => i.MonitoringDevice).Where(i => i.IdentityUserId == userId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Incubators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incubator = await _context.Incubators
                .Include(i => i.IncubatorModel)
                .Include(i => i.MonitoringDevice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incubator == null)
            {
                return NotFound();
            }

            return View(incubator);
        }

        // GET: Incubators/Create
        public IActionResult Create()
        {
            ViewData["IncubatorModelId"] = new SelectList(_context.Set<IncubatorModel>(), "Id", "Capacity");
            ViewBag.NoMonitoringDevice = false;
            return View();

        }

        // POST: Incubators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncubatorModelId,MonitoringDeviceId,Id,Name,Description,IdentityUserId")] Incubator incubator)
        {
            var monitoringDevices = _context.MonitoringDevices.Select(m => m.Id).ToList();

            var incubatorsMonitoringDevices = _context.Incubators.Select(i => i.MonitoringDeviceId).ToList();

            if (monitoringDevices.Contains(incubator.MonitoringDeviceId) || (incubator.MonitoringDeviceId != 0 && incubatorsMonitoringDevices.Contains(incubator.MonitoringDeviceId)) {
                ViewBag.NoMonitoringDevice = true;
                ViewData["IncubatorModelId"] = new SelectList(_context.Set<IncubatorModel>(), "Id", "Capacity", incubator.IncubatorModelId);
                return View(incubator);
                }

            if (incubator.MonitoringDeviceId == 0)
                incubator.MonitoringDeviceId = 2;

            incubator.IdentityUser = await GetCurrentUserAsync();
            incubator.IdentityUserId = await GetCurrentUserId();
            if (ModelState.IsValid)
            {
                var dateAdded = DateTime.Now.Date;

                _context.Add(incubator);

                await _context.SaveChangesAsync();

                incubator = _context.Incubators.Include(i => i.IncubatorModel).SingleOrDefault(i => i.Id == incubator.Id);
                for (byte rack = 1; rack <= incubator.IncubatorModel.RackHeight; rack++)
                {
                    var rackIn = new Rack
                    {
                        IncubatorId = incubator.Id,
                        RackNumber = rack,

                    };

                    _context.Add(rackIn);
                    await _context.SaveChangesAsync();

                    for (byte rackCol = 1; rackCol <= incubator.IncubatorModel.RackLength; rackCol++)
                    {
                        for (byte rackRow = 1; rackRow <= incubator.IncubatorModel.RackWidth; rackRow++)
                        {
                            var tray = new Tray
                            {
                                Column = rackCol,
                                Row = rackRow,
                                EggTypeId = _context.EggTypes.SingleOrDefault(eT => eT.Name == "None").Id,
                                RackId = _context.Racks.Where(r => r.IncubatorId == incubator.Id).SingleOrDefault(r => r.RackNumber == rack).Id,
                                DateAdded = DateTime.Now.Date,
                                CandlingDate = DateTime.Now.Date,
                                HatchPreparationDate = DateTime.Now.Date,
                                HatchDate = DateTime.Now.Date
                            };

                            _context.Add(tray);
                            await _context.SaveChangesAsync();

                        }
                    }

                }

                return RedirectToAction(nameof(Index));

            }
            
            ViewData["IncubatorModelId"] = new SelectList(_context.Set<IncubatorModel>(), "Id", "Capacity", incubator.IncubatorModelId);
            return View(incubator);
        }

        // GET: Incubators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incubator = await _context.Incubators.FindAsync(id);
            if (incubator == null)
            {
                return NotFound();
            }
            ViewData["IncubatorModelId"] = new SelectList(_context.Set<IncubatorModel>(), "Id", "Capacity", incubator.IncubatorModelId);
            return View(incubator);
        }

        // POST: Incubators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncubatorModelId,MonitoringDeviceId,Id,Name,Description,IdentityUserId")] Incubator incubator)
        {
            incubator.IdentityUser = await GetCurrentUserAsync();
            incubator.IdentityUserId = await GetCurrentUserId();
            if (id != incubator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incubator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncubatorExists(incubator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IncubatorModelId"] = new SelectList(_context.Set<IncubatorModel>(), "Id", "Capacity", incubator.IncubatorModelId);
            return View(incubator);
        }

        // GET: Incubators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incubator = await _context.Incubators
                .Include(i => i.IncubatorModel)
                .Include(i => i.MonitoringDevice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incubator == null)
            {
                return NotFound();
            }

            return View(incubator);
        }

        // POST: Incubators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incubator = await _context.Incubators.FindAsync(id);
            _context.Incubators.Remove(incubator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncubatorExists(int id)
        {
            return _context.Incubators.Any(e => e.Id == id);
        }
    }
}
