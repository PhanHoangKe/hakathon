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
    public class TagsController : Controller
    {
        private readonly DataContext _context;

        public TagsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tags.OrderBy(h => h.TagID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.tags.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tags.Find(id);
            if (delhs == null)
                return NotFound();
            _context.tags.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tags
                          select new SelectListItem()
                          {
                              Text = (m.TagID == 1) ? m.TagName : "-- " + m.TagName,
                              Value = m.TagID.ToString()
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
        public IActionResult Create(tblTags mn)
        {
            if (ModelState.IsValid)
            {
                _context.tags.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tags.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tags
                          select new SelectListItem()
                          {
                              Text = (c.TagID == 1) ? c.TagName : "-- " + c.TagName,
                              Value = c.TagID.ToString()
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
        public IActionResult Edit(tblTags dp)
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