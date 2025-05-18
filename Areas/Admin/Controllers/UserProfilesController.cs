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
    public class UserProfilesController : Controller
    {
        private readonly DataContext _context;

        public UserProfilesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.UserProfiles.OrderBy(h => h.UserID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.UserProfiles.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.UserProfiles.Find(id);
            if (delhs == null)
                return NotFound();
            _context.UserProfiles.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.UserProfiles
                          select new SelectListItem()
                          {
                              Text = (m.UserID == 1) ? m.FullName : "-- " + m.FullName,
                              Value = m.UserID.ToString()
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
        public IActionResult Create(tblUserProfiles mn)
        {
            if (ModelState.IsValid)
            {
                _context.UserProfiles.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.UserProfiles.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.UserProfiles
                          select new SelectListItem()
                          {
                              Text = (c.UserID == 1) ? c.FullName : "-- " + c.FullName,
                              Value = c.UserID.ToString()
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
        public IActionResult Edit(tblUserProfiles dp)
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