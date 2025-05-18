using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using hakathon.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hakathon.Controllers
{

    public class RegisterController : Controller
    {
        private readonly DataContext _context;

        public RegisterController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(tblUsers auser)
        {
            if (auser == null || string.IsNullOrEmpty(auser.UserName) || string.IsNullOrEmpty(auser.Password))
            {
                Functions._Message = "Vui lòng nhập đầy đủ thông tin.";
                return RedirectToAction("Index", "Register");
            }

            // Kiểm tra độ dài mật khẩu >=4 và có ít nhất 1 ký tự đặc biệt
            if (auser.Password.Length < 4 || !System.Text.RegularExpressions.Regex.IsMatch(auser.Password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                Functions._Message = "Mật khẩu phải có ít nhất 4 ký tự và chứa ít nhất 1 ký tự đặc biệt!";
                return RedirectToAction("Index", "Register");
            }

            // Kiểm tra UserName đã tồn tại chưa
            var check = _context.tblUser.FirstOrDefault(u => u.UserName == auser.UserName);
            if (check != null)
            {
                Functions._Message = "Tên đăng nhập đã tồn tại!";
                return RedirectToAction("Index", "Register");
            }

            // Băm mật khẩu bằng PasswordHasher
            var passwordHasher = new PasswordHasher<tblUsers>();
            auser.Password = passwordHasher.HashPassword(auser, auser.Password);

            // Lưu vào CSDL
            _context.tblUser.Add(auser);
            _context.SaveChanges();

            Functions._Message = string.Empty;
            return RedirectToAction("Index", "Login");
        }

    }
}
