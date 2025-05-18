using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace hakathon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // if (!Functions.IsLogin())
            // {
            //     return RedirectToAction("Index", "Login", new { area = "Admin" });
            // }

            // Kiểm tra có phải Admin không
            // if (!Functions.IsAdmin(HttpContext.Session))
            // {
            //     Functions.Logout();
            //     HttpContext.Session.Clear();
            //     return RedirectToAction("Index", "Login", new { area = "Admin"  });
            // }
    


            return View();

          
        }
        public IActionResult Logout()
        {
            // Xóa thông tin người dùng
            Functions._UserID = 0;
            Functions._UserName = string.Empty;
            Functions._Email = string.Empty;
            Functions._Message = string.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}