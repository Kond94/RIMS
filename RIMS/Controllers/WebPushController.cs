using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RIMS.Data;
using WebPush;

namespace WebPushDemo.Controllers
{
    public class WebPushController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        public WebPushController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Send(int? id)
        {
            RecurringJob.AddOrUpdate(
            () => SendNotifications(),
             Cron.Daily);

            return View();
        }

        [HttpPost, ActionName("Send")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int id)
        {
            var payload = Request.Form["payload"];
            var subscription = await _context.PushSubscriptions.SingleOrDefaultAsync(m => m.Id == id);

            string vapidPublicKey = _configuration.GetSection("VapidKeys")["PublicKey"];
            string vapidPrivateKey = _configuration.GetSection("VapidKeys")["PrivateKey"];

            var pushSubscription = new PushSubscription(subscription.PushEndpoint, subscription.PushP256DH, subscription.PushAuth);
            var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

            var webPushClient = new WebPushClient();
            webPushClient.SendNotification(pushSubscription, payload, vapidDetails);

            return View();
        }

        public IActionResult GenerateKeys()
        {
            var keys = VapidHelper.GenerateVapidKeys();
            ViewBag.PublicKey = keys.PublicKey;
            ViewBag.PrivateKey = keys.PrivateKey;
            return View();
        }
        public void SendNotifications()
        {
            var subscribers = _context.PushSubscriptions.Select(s => s.IdentityUserId).ToList();

            var notificationTrays = _context.Trays
                .Where(t => subscribers.Contains(t.Rack.Incubator.IdentityUserId))
                .Where(t => t.CandlingDate == DateTime.Today || t.HatchPreparationDate == DateTime.Today || t.HatchDate == DateTime.Today)
                .Select(t => new { t.Rack.Incubator.Name, t.Rack.Incubator.IdentityUserId, t.Rack.Incubator.IdentityUser.Email});

            foreach (var notification in notificationTrays)
            {
                var payload = "{\"title\":\"Hi " + notification.Email + "\",\"message\":\"" + "The following Incubator: " + notification.Name + "needs attention, please click here to see." + "\"}";
                var subscription = _context.PushSubscriptions.SingleOrDefault(s => s.IdentityUserId == notification.IdentityUserId);

                string vapidPublicKey = _configuration.GetSection("VapidKeys")["PublicKey"];
                string vapidPrivateKey = _configuration.GetSection("VapidKeys")["PrivateKey"];

                var pushSubscription = new PushSubscription(subscription.PushEndpoint, subscription.PushP256DH, subscription.PushAuth);
                var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

                var webPushClient = new WebPushClient();
                webPushClient.SendNotification(pushSubscription, payload, vapidDetails);
            }
        }
    }
}