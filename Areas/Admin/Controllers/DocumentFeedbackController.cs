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
    public class DocumentFeedbackController : Controller
    {
        private readonly DataContext _context;

        public DocumentFeedbackController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblDocumentFeedbacks.OrderBy(h => h.FeedbackID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.tblDocumentFeedbacks.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblDocumentFeedbacks.Find(id);
            if (delhs == null)
                return NotFound();
            _context.tblDocumentFeedbacks.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tblDocumentFeedbacks
                          select new SelectListItem()
                          {
                              Text = (m.FeedbackID == 1) ? m.FeedbackType : "-- " + m.FeedbackType,
                              Value = m.FeedbackID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            dulieuload();
            return View();
        }
        [HttpPost]
        public IActionResult Create(tblDocumentFeedback mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblDocumentFeedbacks.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            dulieuload();

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblDocumentFeedbacks.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tblDocumentFeedbacks
                          select new SelectListItem()
                          {
                              Text = (c.FeedbackID == 1) ? c.FeedbackType : "-- " + c.FeedbackType,
                              Value = c.FeedbackID.ToString()
                          }).ToList();
            dpList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.dpList = dpList;
            dulieuload();
            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblDocumentFeedback dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            dulieuload();
            return View(dp);
        }
        public void dulieuload()
        {
             ViewBag.List1 = new SelectList(_context.tblUser, "UserID", "UserName");

        }
    }
}