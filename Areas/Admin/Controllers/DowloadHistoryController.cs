using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hakathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DowloadHistoryController : Controller
    {

   
    
            private readonly DataContext _context;

            public DowloadHistoryController(DataContext context)
            {
                _context = context;
            }

            public IActionResult Index()
            {
                var hslist = _context.DownloadHistories
                 .Include(h => h.user)
                .Include(h => h.document)
                .OrderBy(h => h.DownloadID).ToList();

                return View(hslist);
            }

            public IActionResult Delete(int? id)
            {
                if (id == null || id == 0)
                    return NotFound();
                var hs = _context.DownloadHistories.Find(id);
                if (id == null)
                    return NotFound();
                return View(hs);
            }
            [HttpPost]
            public IActionResult Delete(int id)
            {
                var delhs = _context.DownloadHistories.Find(id);
                if (delhs == null)
                    return NotFound();
                _context.DownloadHistories.Remove(delhs);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            public IActionResult Create()
            {
                var mnList = (from m in _context.DownloadHistories
                              select new SelectListItem()
                              {
                                  Text = (m.DownloadID == 1) ? m.DownloadDate.ToString() : "-- " + m.DownloadDate,
                                  Value = m.DownloadID.ToString()
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
            public IActionResult Create(tblDownloadHistory mn)
            {
                if (ModelState.IsValid)
                {
                    _context.DownloadHistories.Add(mn);
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

                var dp = _context.DownloadHistories.Find(id);
                if (dp == null)
                    return NotFound();

                var dpList = (from c in _context.DownloadHistories
                              select new SelectListItem()
                              {
                                  Text = (c.DownloadID == 1) ? c.DownloadDate.ToString() : "-- " + c.DownloadDate,
                                  Value = c.DownloadID.ToString()
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
            public IActionResult Edit(tblDownloadHistory dp)
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
                ViewBag.list1 = new SelectList(_context.tblUsers, "UserID", "UserName");
                ViewBag.list2 = new SelectList(_context.tblDocuments, "DocumentID ", "Title");
            }


    }
}