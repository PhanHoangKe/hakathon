using System.Linq;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hakathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly DataContext _context;

        public MenuController(DataContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var mnList = _context.tblMenus.OrderBy(m => m.MenuID).ToList();
            return View(mnList);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var menu = _context.tblMenus.Find(id);
            if (menu == null) return NotFound();
            return View(menu);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delMenu = _context.tblMenus.Find(id);
            if (delMenu == null)
                return NotFound();

            _context.tblMenus.Remove(delMenu);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            var mnList = (from m in _context.tblMenus
                          select new SelectListItem()
                          {
                              Text = (m.Levels == 1) ? m.MenuName : "  " + m.MenuName,
                              Value = m.MenuID.ToString()
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
        public IActionResult Create(tblMenu mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblMenus.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            var mnList = (from m in _context.tblMenus
                          select new SelectListItem()
                          {
                              Text = (m.Levels == 1) ? m.MenuName : "  " + m.MenuName,
                              Value = m.MenuID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "--- select ---",
                Value = "0"
            });
            ViewBag.mnList = mnList;

            return View(mn);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var mn = _context.tblMenus.Find(id);
            if (mn == null)
                return NotFound();

            var mnList = (from m in _context.tblMenus
                          select new SelectListItem()
                          {
                              Text = (m.Levels == 1) ? m.MenuName : "-- " + m.MenuName,
                              Value = m.MenuID.ToString()
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "-- select --",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            return View(mn);
        }

        [HttpPost]
        public IActionResult Edit(tblMenu mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblMenus.Update(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mn);
        }
    }
}
