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
    public class AuthorController : Controller
    {
        private readonly DataContext _context;

        public AuthorController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.tblAuthors.OrderBy(h=>h.AuthorID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.tblAuthors.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblAuthors.Find(id);
            if(delhs==null)
            return NotFound();
            _context.tblAuthors.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tblAuthors
                          select new SelectListItem()
                          {
                              Text = (m.AuthorID == 1) ? m.AuthorName : "-- " + m.AuthorName,
                              Value = m.AuthorID.ToString()
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
        public IActionResult Create(tblAuthors mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblAuthors.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
             
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblAuthors.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tblAuthors
                          select new SelectListItem()
                          {
                              Text = (c.AuthorID == 1) ? c.AuthorName : "-- " + c.AuthorName,
                              Value = c.AuthorID.ToString()
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
        public IActionResult Edit(tblAuthors dp)
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