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
    public class DocumentController : Controller
    {
        private readonly DataContext _context;

        public DocumentController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblDocuments
            .Include(h => h.Category)
            .Include(h => h.Publisher)
            //  .Include(h => h.users)
            .OrderBy(h => h.PublisherID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.tblDocuments.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblDocuments.Find(id);
            if (delhs == null)
                return NotFound();
            _context.tblDocuments.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tblDocuments
                          select new SelectListItem()
                          {
                              Text = (m.DocumentID == 1) ? m.Description : "-- " + m.Description,
                              Value = m.DocumentID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            laydulieu();
            return View();
        }
        [HttpPost]
        public IActionResult Create(tblDocuments mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblDocuments.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            laydulieu();
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblDocuments.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tblDocuments
                          select new SelectListItem()
                          {
                              Text = (c.DocumentID == 1) ? c.Description : "-- " + c.Description,
                              Value = c.DocumentID.ToString()
                          }).ToList();
            dpList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.dpList = dpList;
            laydulieu();
            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblDocuments dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            laydulieu();
            return View(dp);
        }

        private void laydulieu()
        {
            ViewBag.list1 = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewBag.list2 = new SelectList(_context.Publishers, "PublisherID", "PublisherName");
            ViewBag.list3 = new SelectList(_context.tblUsers, "UserID", "UserName");
        }
        //phe duyet
        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            var doc = await _context.tblDocuments.FindAsync(id);
            if (doc == null) return NotFound();

            doc.IsApproved = true;
            doc.ModifiedDate = DateTime.Now;
            _context.Update(doc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Unapprove(int id)
        {
            var doc = await _context.tblDocuments.FindAsync(id);
            if (doc == null) return NotFound();

            doc.IsApproved = false;
            doc.ModifiedDate = DateTime.Now;
            _context.Update(doc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}