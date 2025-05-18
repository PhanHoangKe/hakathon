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
    public class DocumentAuthorsController : Controller
    {
        private readonly DataContext _context;

        public DocumentAuthorsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblDocumentAuthors
                .Include(h => h.Document)
                .Include(h => h.Author)
                .OrderBy(h => h.DocumentID)
                .ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? documentId, int? authorId)
        {
            if (documentId == null || authorId == null)
                return NotFound();

            var hs = _context.tblDocumentAuthors
                .Include(x => x.Document)
                .Include(x => x.Author)
                .FirstOrDefault(x => x.DocumentID == documentId && x.AuthorID == authorId);

            if (hs == null)
                return NotFound();

            dulieu(); // Thêm dòng này để tránh lỗi SelectList null trong view
            return View(hs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int documentId, int authorId)
        {
            var delhs = _context.tblDocumentAuthors
                .FirstOrDefault(x => x.DocumentID == documentId && x.AuthorID == authorId);

            if (delhs == null)
                return NotFound();

            _context.tblDocumentAuthors.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            dulieu();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(tblDocumentAuthors mn)
        {
            var exists = _context.tblDocumentAuthors
                .Any(x => x.DocumentID == mn.DocumentID && x.AuthorID == mn.AuthorID);

            if (exists)
            {
                ModelState.AddModelError("", "Tác giả này đã được gán cho tài liệu này.");
                dulieu();
                return View(mn);
            }

            if (ModelState.IsValid)
            {
                _context.tblDocumentAuthors.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            dulieu();
            return View(mn);
        }

        public IActionResult Edit(int? documentId, int? authorId)
        {
            if (documentId == null || authorId == null)
                return NotFound();

            var dp = _context.tblDocumentAuthors
                .FirstOrDefault(x => x.DocumentID == documentId && x.AuthorID == authorId);

            if (dp == null)
                return NotFound();

            dulieu();
            return View(dp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(tblDocumentAuthors dp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.tblDocumentAuthors.Update(dp);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Không thể cập nhật. Có thể đã có bản ghi với khóa trùng.");
                }
            }

            dulieu();
            return View(dp);
        }

        public void dulieu()
        {
            var authors = _context.tblAuthors.ToList();
            var documents = _context.tblDocuments.ToList();
            ViewBag.list1 = new SelectList(_context.tblAuthors, "AuthorID", "AuthorName");
            ViewBag.list2 = new SelectList(_context.tblDocuments, "DocumentID", "Title");
        }
    }
}
