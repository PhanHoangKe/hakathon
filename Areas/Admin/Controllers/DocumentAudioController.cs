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
    public class DocumentAudioController : Controller
    {
       private readonly DataContext _context;

        public DocumentAudioController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.DocumentAudios
            .Include(h=>h.document)
            .OrderBy(h=>h.AudioID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.DocumentAudios.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.DocumentAudios.Find(id);
            if(delhs==null)
            return NotFound();
            _context.DocumentAudios.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.DocumentAudios
                          select new SelectListItem()
                          {
                              Text = (m.AudioID == 1) ? m.AudioPath : "-- " + m.AudioPath,
                              Value = m.AudioID.ToString()
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
        public IActionResult Create(tblDocumentAudio mn)
        {
            if (ModelState.IsValid)
            {
                _context.DocumentAudios.Add(mn);
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

            var dp = _context.DocumentAudios.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.DocumentAudios
                          select new SelectListItem()
                          {
                              Text = (c.AudioID == 1) ? c.AudioPath : "-- " + c.AudioPath,
                              Value = c.AudioID.ToString()
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
        public IActionResult Edit(tblDocumentAudio dp)
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
            ViewBag.list1 = new SelectList(_context.tblDocuments, "DocumentID ", "Title");
        }
    }
}