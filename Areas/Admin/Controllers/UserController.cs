// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using hakathon.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.Extensions.Logging;

// namespace hakathon.Areas.Admin.Controllers
// {
//     [Area("Admin")]
//     public class UserController : Controller
//     {
//         private readonly DataContext _context;

//         public UserController(DataContext context)
//         {
//             _context = context;
//         }

//         public IActionResult Index()
//         {
//             var hslist = _context.tblUser.OrderBy(h => h.UserID).ToList();

//             return View(hslist);
//         }

//         public IActionResult Delete(int? id)
//         {
//             if (id == null || id == 0)
//                 return NotFound();
//             var hs = _context.tblUser.Find(id);
//             if (id == null)
//                 return NotFound();
//             return View(hs);
//         }
//         [HttpPost]
//         public IActionResult Delete(int id)
//         {
//             var delhs = _context.tblUser.Find(id);
//             if (delhs == null)
//                 return NotFound();
//             _context.tblUser.Remove(delhs);
//             _context.SaveChanges();
//             return RedirectToAction("Index");
//         }
//         public IActionResult Create()
//         {
//             var mnList = (from m in _context.tblUser
//                           select new SelectListItem()
//                           {
//                               Text = (m.UserID == 1) ? m.UserName : "-- " + m.UserID,
//                               Value = m.UserID.ToString()
//                           }).ToList();
//             mnList.Insert(0, new SelectListItem()
//             {
//                 Text = "--- select ---",
//                 Value = "0"
//             });
//             ViewBag.mnList = mnList;

//             return View();
//         }
//         [HttpPost]
//         public IActionResult Create(tblUsers mn)
//         {
//             if (ModelState.IsValid)
//             {
//                 var passwordHasher = new PasswordHasher<tblUsers>();
//                 mn.Password = passwordHasher.HashPassword(mn, mn.Password); // Băm mật khẩu với salt
//                 _context.tblUser.Add(mn);
//                 _context.SaveChanges();
//                 return RedirectToAction("Index");
//             }

//             return View(mn);
//         }

//         public IActionResult Edit(int? id)
//         {
//             if (id == null || id == 0)
//                 return NotFound();

//             var mn = _context.tblUser.Find(id);
//             if (mn == null)
//                 return NotFound();

//             var mnList = (from m in _context.tblUser
//                           select new SelectListItem()
//                           {
//                               Text = (m.UserID == 1) ? m.UserName : "-- " + m.UserID,
//                               Value = m.UserID.ToString()
//                           }).ToList();

//             mnList.Insert(0, new SelectListItem()
//             {
//                 Text = "--- select ---",
//                 Value = "0"
//             });

//             ViewBag.mnList = mnList;

//             return View(mn); // Trả về giao diện chỉnh sửa tài khoản
//         }

//         [HttpPost]


//         public IActionResult Edit(tblUsers mn)
//         {
//             if (!ModelState.IsValid || mn == null)
//             {
//                 return View(mn); // Trả lại form nếu dữ liệu không hợp lệ
//             }

//             var acc = _context.tblUser.FirstOrDefault(u => u.UserID == mn.UserID);
//             if (acc == null)
//             {
//                 return NotFound(); // Không tìm thấy tài khoản
//             }

//             // Cập nhật các thông tin cơ bản
//             acc.UserName = mn.UserName;
//             acc.Email = mn.Email;

//             // Nếu mật khẩu thay đổi và không rỗng, mã hóa lại
//             if (!string.IsNullOrEmpty(mn.Password) && mn.Password != acc.Password)
//             {
//                 var passwordHasher = new PasswordHasher<tblUsers>();
//                 acc.Password = passwordHasher.HashPassword(acc, mn.Password); // Băm mật khẩu mới
//             }

//             // Cập nhật trạng thái tài khoản
//             acc.IsActive = mn.IsActive;


//             _context.tblUser.Update(acc);
//             _context.SaveChanges();

//             return RedirectToAction("Index");
//         }

//     }
// }

using System;
using System.Linq;
using System.Text.RegularExpressions;
using hakathon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hakathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hslist = _context.tblUser.OrderBy(h => h.UserID).ToList();
            return View(hslist);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var hs = _context.tblUser.Find(id);
            if (hs == null)
                return NotFound();

            return View(hs);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delhs = _context.tblUser.Find(id);
            if (delhs == null)
                return NotFound();

            _context.tblUser.Remove(delhs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            LoadUserDropdown();
            return View();
        }

        [HttpPost]
        public IActionResult Create(tblUsers mn)
        {
            if (!IsValidPassword(mn.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 4 ký tự và chứa ít nhất 1 ký tự đặc biệt.");
            }

            if (!ModelState.IsValid)
            {
                LoadUserDropdown();
                return View(mn);
            }

            var passwordHasher = new PasswordHasher<tblUsers>();
            mn.Password = passwordHasher.HashPassword(mn, mn.Password);
            _context.tblUser.Add(mn);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var mn = _context.tblUser.Find(id);
            if (mn == null)
                return NotFound();

            LoadUserDropdown();
            return View(mn);
        }

        [HttpPost]
        public IActionResult Edit(tblUsers mn)
        {
            var acc = _context.tblUser.FirstOrDefault(u => u.UserID == mn.UserID);
            if (acc == null)
                return NotFound();

            // Nếu mật khẩu được thay đổi
            if (!string.IsNullOrEmpty(mn.Password) && mn.Password != acc.Password)
            {
                if (!IsValidPassword(mn.Password))
                {
                    ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 4 ký tự và chứa ít nhất 1 ký tự đặc biệt.");
                }
                else
                {
                    var passwordHasher = new PasswordHasher<tblUsers>();
                    acc.Password = passwordHasher.HashPassword(acc, mn.Password);
                }
            }

            if (!ModelState.IsValid)
            {
                LoadUserDropdown();
                return View(mn);
            }

            acc.UserName = mn.UserName;
            acc.Email = mn.Email;
            acc.IsActive = mn.IsActive;

            _context.tblUser.Update(acc);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================== Helper Methods ==================

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password)
                && password.Length >= 4
                && Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");
        }

        private void LoadUserDropdown()
        {
            var mnList = _context.tblUser.Select(m => new SelectListItem
            {
                Text = (m.UserID == 1) ? m.UserName : "-- " + m.UserID,
                Value = m.UserID.ToString()
            }).ToList();

            mnList.Insert(0, new SelectListItem
            {
                Text = "--- select ---",
                Value = "0"
            });

            ViewBag.mnList = mnList;
        }
    }
}
