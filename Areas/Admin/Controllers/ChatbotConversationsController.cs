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
    public class ChatbotConversationsController : Controller
    {
                private readonly DataContext _context;

        public ChatbotConversationsController(DataContext context)
        {
            _context = context;
        }

       public IActionResult Index()
        { 
            var hslist = _context.chatbotConversations.OrderBy(h=>h.ConversationID).ToList();
            
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
             if(id==null|| id==0)
             return NotFound();
             var hs = _context.chatbotConversations.Find(id);
             if(id==null)
              return NotFound();
              return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.chatbotConversations.Find(id);
            if(delhs==null)
            return NotFound();
            _context.chatbotConversations.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.chatbotConversations
                          select new SelectListItem()
                          {
                              Text = (m.ConversationID == 1) ? m.UserID.ToString() : "-- " + m.UserID.ToString(),
                              Value = m.ConversationID.ToString()
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
        public IActionResult Create(tblChatbotConversations mn)
        {
            if (ModelState.IsValid)
            {
                _context.chatbotConversations.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
             
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.chatbotConversations.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.chatbotConversations
                          select new SelectListItem()
                          {
                              Text = (c.ConversationID == 1) ? c.UserID.ToString() : "-- " + c.UserID.ToString(),
                              Value = c.ConversationID.ToString()
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
        public IActionResult Edit(tblChatbotConversations dp)
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