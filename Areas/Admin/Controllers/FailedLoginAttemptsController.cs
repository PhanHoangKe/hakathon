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
    public class FailedLoginAttemptsController : Controller
    {
                 private readonly DataContext _context;

        public FailedLoginAttemptsController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.FailedLoginAttempts.OrderBy(h=>h.AttemptID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.FailedLoginAttempts.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.FailedLoginAttempts.Find(id);
            if(delhs==null)
            return NotFound();
            _context.FailedLoginAttempts.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.FailedLoginAttempts
                          select new SelectListItem()
                          {
                              Text = (m.AttemptID == 1) ? m.Username : "-- " + m.Username,
                              Value = m.AttemptID.ToString()
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
        public IActionResult Create(tblFailedLoginAttempts mn)
        {
            if (ModelState.IsValid)
            {
                _context.FailedLoginAttempts.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
             
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.FailedLoginAttempts.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.FailedLoginAttempts
                          select new SelectListItem()
                          {
                              Text = (c.AttemptID == 1) ? c.Username : "-- " + c.Username,
                              Value = c.AttemptID.ToString()
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
        public IActionResult Edit(tblFailedLoginAttempts dp)
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