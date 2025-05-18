using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace hakathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserNotificationsController : Controller
    {
        private readonly DataContext _context;

        public UserNotificationsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblUserNotifications.OrderBy(h => h.NotificationID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.tblUserNotifications.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblUserNotifications.Find(id);
            if (delhs == null)
                return NotFound();
            _context.tblUserNotifications.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tblUserNotifications
                          select new SelectListItem()
                          {
                              Text = (m.NotificationID == 1) ? m.NotificationType : "-- " + m.NotificationType,
                              Value = m.NotificationID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;

            return View();
        }
        [HttpPost]
        public IActionResult Create(tblUserNotifications mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblUserNotifications.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblUserNotifications.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tblUserNotifications
                          select new SelectListItem()
                          {
                              Text = (c.NotificationID == 1) ? c.NotificationType : "-- " + c.NotificationType,
                              Value = c.NotificationID.ToString()
                          }).ToList();
            dpList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.dpList = dpList;

            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblUserNotifications dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dp);
        }
    }
}