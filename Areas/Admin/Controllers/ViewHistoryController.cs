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
    public class ViewHistoryController : Controller
    {
        private readonly DataContext _context;

        public ViewHistoryController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.viewHistories.OrderBy(h => h.ViewID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.viewHistories.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.viewHistories.Find(id);
            if (delhs == null)
                return NotFound();
            _context.viewHistories.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.viewHistories
                          select new SelectListItem()
                          {
                              Text = (m.ViewID == 1) ? m.UserID.ToString() : "-- " + m.UserID.ToString(),
                              Value = m.ViewID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            dulieu();
            return View();
        }
        [HttpPost]
        public IActionResult Create(tblViewHistory mn)
        {
            if (ModelState.IsValid)
            {
                _context.viewHistories.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            dulieu();
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.viewHistories.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.viewHistories
                          select new SelectListItem()
                          {
                              Text = (c.ViewID == 1) ? c.UserID.ToString() : "-- " + c.UserID.ToString(),
                              Value = c.ViewID.ToString()
                          }).ToList();
            dpList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.dpList = dpList;
            dulieu();
            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblViewHistory dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            dulieu();
            return View(dp);
        }
        public void dulieu()
        {
            ViewBag.list1 = new SelectList(_context.tblUser, "UserID", "UserName");
            ViewBag.list2 = new SelectList(_context.tblDocuments, "DocumentID ", "Title");
        }
    }
}