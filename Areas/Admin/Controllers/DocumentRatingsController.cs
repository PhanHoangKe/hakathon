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
    public class DocumentRatingsController : Controller
    {
       private readonly DataContext _context;

        public DocumentRatingsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblDocumentRatings.OrderBy(h => h.RatingID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.tblDocumentRatings.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblDocumentRatings.Find(id);
            if (delhs == null)
                return NotFound();
            _context.tblDocumentRatings.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tblDocumentRatings
                          select new SelectListItem()
                          {
                              Text = (m.RatingID == 1) ? m.Comment : "-- " + m.Comment,
                              Value = m.RatingID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
    ViewBag.List1 = new SelectList(_context.UserProfiles, "UserID", "FullName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(tblDocumentRatings mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblDocumentRatings.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
    ViewBag.List1 = new SelectList(_context.UserProfiles, "UserID", "FullName");
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblDocumentRatings.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tblDocumentRatings
                          select new SelectListItem()
                          {
                              Text = (c.RatingID == 1) ? c.Comment : "-- " + c.Comment,
                              Value = c.RatingID.ToString()
                          }).ToList();
            dpList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.dpList = dpList;
    ViewBag.List1 = new SelectList(_context.UserProfiles, "UserID", "FullName");
            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblDocumentRatings dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.List1 = new SelectList(_context.UserProfiles, "UserID", "FullName");
          
       
            return View(dp);
        }

    }
}