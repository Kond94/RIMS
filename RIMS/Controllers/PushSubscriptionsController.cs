using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RIMS.Data;
using RIMS.Models;

namespace RIMS.Controllers
{
    public class PushSubscriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public PushSubscriptionsController(ApplicationDbContext _context, IConfiguration configuration)
        {
            this._context = _context;
            _configuration = configuration;
        }

        // GET: pushSubscriptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.PushSubscriptions.ToListAsync());
        }

        // GET: pushSubscriptions/Create
        public IActionResult Create()
        {
            ViewBag.PublicKey = _configuration.GetSection("VapidKeys")["PublicKey"];

            return View();
        }

        // POST: PushSubscriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PushEndpoint,PushP256DH,PushAuth")] PushSubscription pushSubscriptions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pushSubscriptions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(pushSubscriptions);
        }

        // GET: pushSubscriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pushSubscriptions = await _context.PushSubscriptions
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pushSubscriptions == null)
            {
                return NotFound();
            }

            return View(pushSubscriptions);
        }

        // POST: pushSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pushSubscription = await _context.PushSubscriptions.SingleOrDefaultAsync(m => m.Id == id);
            _context.PushSubscriptions.Remove(pushSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
