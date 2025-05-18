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
    public class PublishersController : Controller
    {
       private readonly DataContext _context;

        public PublishersController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.Publishers.OrderBy(h=>h.PublisherID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.Publishers.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.Publishers.Find(id);
            if(delhs==null)
            return NotFound();
            _context.Publishers.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.Publishers
                          select new SelectListItem()
                          {
                              Text = (m.PublisherID == 1) ? m.PublisherName : "-- " + m.PublisherName,
                              Value = m.PublisherID.ToString()
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
        public IActionResult Create(tblPublishers mn)
        {
            if (ModelState.IsValid)
            {
                _context.Publishers.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
             
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.Publishers.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.Publishers
                          select new SelectListItem()
                          {
                              Text = (c.PublisherID == 1) ? c.PublisherName : "-- " + c.PublisherName,
                              Value = c.PublisherID.ToString()
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
        public IActionResult Edit(tblPublishers dp)
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