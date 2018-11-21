using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RIMS.Data;
using RIMS.Models;

namespace RIMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            IdentityUser usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserId();
            var isSubscribed = false;
            var hasIncubator = false;

            if (_context.PushSubscriptions.Where(pS => pS.IdentityUserId == userId).Any())
            {
                isSubscribed = true;
            }

            if (_context.Incubators.Where(i => i.IdentityUserId == userId).Any())
            {
                hasIncubator = true;
            }

            ViewBag.PublicKey = _configuration.GetSection("VapidKeys")["PublicKey"];
            ViewBag.isSubscribed = isSubscribed;
            ViewBag.hasIncubator = hasIncubator;

            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "RIMS description ikhale apa.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact details apa.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubscription([Bind("Id,IdentityUserId,PushEndpoint,PushP256DH,PushAuth")] PushSubscription pushSubscription)
        {
            pushSubscription.IdentityUserId = await GetCurrentUserId();

            if (ModelState.IsValid)
            {
                _context.Add(pushSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
