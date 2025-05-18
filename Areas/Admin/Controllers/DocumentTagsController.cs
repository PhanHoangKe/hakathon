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
    public class DocumentTagsController : Controller
    {
        private readonly DataContext _context;

        public DocumentTagsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblDocumentTag
            .Include(h => h.tag)
            .Include(h => h.document)
            .OrderBy(h => h.DocumentID).ToList();

            return View(hslist);
        }

        // GET: Delete confirmation page
        public IActionResult Delete(int? documentId, int? tagId)
        {
            if (documentId == null || tagId == null)
                return NotFound();

            var hs = _context.tblDocumentTag
                .Include(h => h.document)
                .Include(h => h.tag)
                .FirstOrDefault(x => x.DocumentID == documentId && x.TagID == tagId);

            if (hs == null)
                return NotFound();

            return View(hs);
        }

        // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int documentId, int tagId)
        {
            var delhs = _context.tblDocumentTag
                .FirstOrDefault(x => x.DocumentID == documentId && x.TagID == tagId);

            if (delhs == null)
                return NotFound();

            _context.tblDocumentTag.Remove(delhs);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(tblDocumentTags mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblDocumentTag.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblDocumentTag.Find(id);
            if (dp == null)
                return NotFound();

            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblDocumentTags dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dp);
        }
        public void dulieu()
        {
            ViewBag.List1 = new SelectList(_context.tblDocuments, "DocumentID ", "Title");
            ViewBag.tag1= new SelectList(_context.tags, "TagID ", "TagName");
        }

    }
}