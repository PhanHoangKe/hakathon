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
    public class CommentsController : Controller
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.Comments
            .Include(h=>h.document)
            .Include(h=>h.user)
            .OrderBy(h=>h.CommentID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.Comments.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.Comments.Find(id);
            if(delhs==null)
            return NotFound();
            _context.Comments.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.Comments
                          select new SelectListItem()
                          {
                              Text = (m.CommentID == 1) ? m.CommentText : "-- " + m.CommentText,
                              Value = m.CommentID.ToString()
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
        public IActionResult Create(tblComments mn)
        {
            if (ModelState.IsValid)
            {
                _context.Comments.Add(mn);
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

            var dp = _context.Comments.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.Comments
                          select new SelectListItem()
                          {
                              Text = (c.CommentID == 1) ? c.CommentText : "-- " + c.CommentText,
                              Value = c.CommentID.ToString()
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
        public IActionResult Edit(tblComments dp)
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
        public void dulieu(){
            ViewBag.List1 = new SelectList(_context.tblUser, "UserID", "UserName");
            ViewBag.List2 = new SelectList(_context.tblDocuments, "DocumentID ", "Title");
        }
    }
}