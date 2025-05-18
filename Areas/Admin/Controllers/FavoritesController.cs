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
    public class FavoritesController : Controller
    {
        private readonly DataContext _context;

        public FavoritesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblFavorites.OrderBy(h => h.FavoriteID).ToList();

            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var hs = _context.tblFavorites.Find(id);
            if (id == null)
                return NotFound();
            return View(hs);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblFavorites.Find(id);
            if (delhs == null)
                return NotFound();
            _context.tblFavorites.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.tblFavorites
                          select new SelectListItem()
                          {
                              Text = (m.FavoriteID == 1) ? m.UserID.ToString() : "-- " + m.UserID.ToString(),
                              Value = m.FavoriteID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            dulieuload();
            return View();
        }
        [HttpPost]
        public IActionResult Create(tblFavorites mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblFavorites.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            dulieuload();
            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblFavorites.Find(id);
            if (dp == null)
                return NotFound();

            var dpList = (from c in _context.tblFavorites
                          select new SelectListItem()
                          {
                              Text = (c.FavoriteID == 1) ? c.UserID.ToString() : "-- " + c.UserID.ToString(),
                              Value = c.FavoriteID.ToString()
                          }).ToList();
            dpList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.dpList = dpList;
            dulieuload();
            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblFavorites dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            dulieuload();
            return View(dp);
        }
         public void dulieuload(){
            ViewBag.List1 = new SelectList(_context.tblUser, "UserID", "UserName");
            ViewBag.List2 = new SelectList(_context.tblDocuments, "DocumentID ", "Title");
        }
    }
}