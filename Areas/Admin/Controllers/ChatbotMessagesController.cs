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
    public class ChatbotMessagesController : Controller
    {
          private readonly DataContext _context;

        public ChatbotMessagesController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.ChatbotMessages.OrderBy(h=>h.MessageID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.ChatbotMessages.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.ChatbotMessages.Find(id);
            if(delhs==null)
            return NotFound();
            _context.ChatbotMessages.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.ChatbotMessages
                          select new SelectListItem()
                          {
                              Text = (m.MessageID == 1) ? m.MessageText : "-- " + m.MessageText,
                              Value = m.MessageID.ToString()
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
        public IActionResult Create(tblChatbotMessages mn)
        {
            if (ModelState.IsValid)
            {
                _context.ChatbotMessages.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
             
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.ChatbotMessages.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.ChatbotMessages
                          select new SelectListItem()
                          {
                              Text = (c.MessageID == 1) ? c.MessageText : "-- " + c.MessageText,
                              Value = c.MessageID.ToString()
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
        public IActionResult Edit(tblChatbotMessages dp)
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