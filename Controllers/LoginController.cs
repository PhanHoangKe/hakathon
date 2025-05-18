using hakathon.Models;
using hakathon.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace hakathon.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        private const string LoginAttemptCountKey = "LoginAttemptCount";

        public LoginController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Kiểm tra nếu đã đăng nhập thì chuyển hướng đến trang phù hợp với vai trò
            if (User.Identity.IsAuthenticated)
            {
                return RedirectBasedOnUserRole();
            }
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool remember = false, string captchaInput = null)
        {
            // Lấy số lần đăng nhập sai từ session
            int failedAttempts = HttpContext.Session.GetInt32(LoginAttemptCountKey) ?? 0;

            // Kiểm tra nếu đã đăng nhập sai 3 lần trở lên thì yêu cầu CAPTCHA
            bool captchaRequired = failedAttempts >= 3;

            // Kiểm tra thông tin đăng nhập
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "Vui lòng nhập tên đăng nhập và mật khẩu." });
            }

            // Kiểm tra CAPTCHA nếu cần thiết
            if (captchaRequired && string.IsNullOrEmpty(captchaInput))
            {
                return Json(new { success = false, message = "Vui lòng nhập mã xác nhận CAPTCHA." });
            }

            // Tìm người dùng theo tên đăng nhập hoặc email
            var user = await _context.tblUsers
                .FirstOrDefaultAsync(u => (u.UserName == username || u.Email == username) && u.IsActive == true);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                // Tăng số lần đăng nhập sai
                failedAttempts++;
                HttpContext.Session.SetInt32(LoginAttemptCountKey, failedAttempts);

                return Json(new
                {
                    success = false,
                    message = "Tên đăng nhập hoặc mật khẩu không chính xác.",
                    failedAttempts = failedAttempts,
                    captchaRequired = failedAttempts >= 3
                });
            }

            // Đăng nhập thành công, reset số lần đăng nhập sai
            HttpContext.Session.Remove(LoginAttemptCountKey);

            // Lấy vai trò của người dùng
            var userRoles = await _context.tblUsersRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserID == user.UserID)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();

            // Tạo các claim để lưu thông tin người dùng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Thêm vai trò vào claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Tạo identity và principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);

            // Thiết lập thời gian tồn tại của cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = remember, // Ghi nhớ đăng nhập nếu được chọn
                ExpiresUtc = remember ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
            };

            // Đăng nhập người dùng
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // Xác định URL chuyển hướng dựa trên vai trò người dùng
            string redirectUrl = GetRedirectUrlBasedOnRole(userRoles);

            return Json(new { success = true, redirect = redirectUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Xóa thông tin đăng nhập sai khỏi session
            HttpContext.Session.Remove(LoginAttemptCountKey);

            // Gửi yêu cầu reset Tawk.to khi người dùng đăng xuất
            TempData["ResetTawk"] = true;

            // Đăng xuất tài khoản hiện tại và xóa cookie tương ứng
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Trả về kết quả JSON với đường dẫn chuyển hướng tương ứng
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirect = "http://localhost:5186" });
            }

            // Trong trường hợp không phải Ajax request, chuyển hướng trực tiếp
            return Redirect("http://localhost:5186");
        }

        [HttpGet]
        public IActionResult IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    isAuthenticated = true,
                    username = User.Identity.Name,
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    roles = User.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList()
                });
            }
            return Json(new { isAuthenticated = false });
        }

        // Phương thức kiểm tra mật khẩu
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Trong thực tế, cần sử dụng phương thức mã hóa như BCrypt hoặc PBKDF2
            var passwordHasher = new PasswordHasher<tblUsers>();
            var result = passwordHasher.VerifyHashedPassword(null, storedPassword, inputPassword);
            return result == PasswordVerificationResult.Success;
            // Tạm thời so sánh trực tiếp để demo

            //return inputPassword == storedPassword;
        }

        // Phương thức lấy URL chuyển hướng dựa trên vai trò người dùng
        private string GetRedirectUrlBasedOnRole(List<string> roles)
        {
            // Kiểm tra vai trò và chuyển hướng tương ứng
            if (roles.Contains("Admin"))
            {
                return "http://localhost:5186/admin";
            }

            // Nếu không có vai trò đặc biệt, chuyển về trang chủ
            return "http://localhost:5186";
        }

        // Phương thức chuyển hướng dựa trên vai trò của người dùng đã đăng nhập
        private IActionResult RedirectBasedOnUserRole()
        {
            // Lấy danh sách vai trò của người dùng từ claims
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            // Kiểm tra vai trò và chuyển hướng tương ứng
            if (roles.Contains("Admin"))
            {
                return Redirect("http://localhost:5186/admin");
            }

            // Mặc định chuyển về trang chủ
            return Redirect("http://localhost:5186");
        }
    }
}

