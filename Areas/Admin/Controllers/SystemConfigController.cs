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
    public class SystemConfigController : Controller
    {
          private readonly DataContext _context;

        public SystemConfigController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.SystemConfigs.OrderBy(h=>h.ConfigID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.SystemConfigs.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.SystemConfigs.Find(id);
            if(delhs==null)
            return NotFound();
            _context.SystemConfigs.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.SystemConfigs
                          select new SelectListItem()
                          {
                              Text = (m.ConfigID == 1) ? m.ConfigKey : "-- " + m.ConfigKey,
                              Value = m.ConfigID.ToString()
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
        public IActionResult Create(tblSystemConfig mn)
        {
            if (ModelState.IsValid)
            {
                _context.SystemConfigs.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
             
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.SystemConfigs.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.SystemConfigs
                          select new SelectListItem()
                          {
                              Text = (c.ConfigID == 1) ? c.ConfigKey : "-- " + c.ConfigKey,
                              Value = c.ConfigID.ToString()
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
        public IActionResult Edit(tblSystemConfig dp)
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