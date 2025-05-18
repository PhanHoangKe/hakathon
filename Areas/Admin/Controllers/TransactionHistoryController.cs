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
    public class TransactionHistoryController : Controller
    {
         private readonly DataContext _context;

        public TransactionHistoryController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.TransactionHistories.OrderBy(h => h.TransactionID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.TransactionHistories.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.TransactionHistories.Find(id);
            if (delhs == null)
                return NotFound();
            _context.TransactionHistories.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.TransactionHistories
                          select new SelectListItem()
                          {
                              Text = (m.TransactionID == 1) ? m.ActionType : "-- " + m.ActionType,
                              Value = m.TransactionID.ToString()
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
        public IActionResult Create(tblTransactionHistory mn)
        {
            if (ModelState.IsValid)
            {
                _context.TransactionHistories.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.TransactionHistories.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.TransactionHistories
                          select new SelectListItem()
                          {
                              Text = (c.TransactionID == 1) ? c.ActionType : "-- " + c.ActionType,
                              Value = c.TransactionID.ToString()
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
        public IActionResult Edit(tblTransactionHistory dp)
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