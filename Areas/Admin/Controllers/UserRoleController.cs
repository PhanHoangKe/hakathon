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
    public class UserRoleController : Controller
    {
        private readonly DataContext _context;

        public UserRoleController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblUsersRoles
              .Include(h => h.User)
                .Include(h => h.Role)
            .OrderBy(h => h.UserID).ToList();

            return View(hslist);
        }

        // GET: Delete confirmation page
        public IActionResult Delete(int? userID, int? roleID)
        {
            if (userID == null || roleID == null)
                return NotFound();

            var hs = _context.tblUsersRoles
                .Include(h => h.User)
                .Include(h => h.Role)
                .FirstOrDefault(x => x.UserID == userID && x.RoleID == roleID);

            if (hs == null)
                return NotFound();

            return View(hs);
        }

        // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int userID, int roleID)
        {
            var delhs = _context.tblUsersRoles
                .FirstOrDefault(x => x.UserID == userID && x.RoleID == roleID);

            if (delhs == null)
                return NotFound();

            _context.tblUsersRoles.Remove(delhs);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public IActionResult Create(tblUsersRoles mn)
        {
            if (ModelState.IsValid)
            {
                _context.tblUsersRoles.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            laydulieu();

            return View(mn);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var dp = _context.tblUsersRoles.Find(id);
            if (dp == null)
                return NotFound();


            laydulieu();
            return View(dp);
        }
        [HttpPost]
        public IActionResult Edit(tblUsersRoles dp)
        {
            if (ModelState.IsValid)
            {
                _context.Update(dp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            laydulieu();
            return View(dp);
        }
        private void laydulieu()
        {
            ViewBag.list1 = new SelectList(_context.tblRoles, "RoleID", "RoleName");

        }
    }
}