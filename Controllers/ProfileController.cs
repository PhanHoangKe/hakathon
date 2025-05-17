using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hakathon.Controllers
{
	public class ProfileController : Controller
	{

		private readonly DataContext _context;

		public ProfileController(DataContext context)
		{
			_context = context;
		}


        [HttpGet]
        public IActionResult Profile(string activeTab = "sensy-profile-tab")
        {
            TempData["ActiveTab"] = activeTab;
            return View("Profile");
        }


        [HttpGet]
        public async Task<IActionResult> Create(int userId)
        {
            var model = await _context.tblUserProfiles.FirstOrDefaultAsync(x => x.UserID == userId)
                         ?? new tblUserProfiles { UserID = userId };

            return View("Create", model); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(tblUserProfiles profile)
        {
            string? userIdStr = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized(); 
            }

            profile.UserID = userId;

            if (ModelState.IsValid)
            {
                _context.tblUserProfiles.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Profile", "Profile");
            }

            return View("Create", profile);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(tblUserProfiles profile)
        {
            string? userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized();
            }

            var existingProfile = await _context.tblUserProfiles.FirstOrDefaultAsync(p => p.UserID == userId);
            if (existingProfile == null)
            {
                return NotFound();
            }

            existingProfile.FullName = profile.FullName;
            existingProfile.Bio = profile.Bio;
            existingProfile.Phone = profile.Phone;
            existingProfile.Address = profile.Address;
            existingProfile.DateOfBirth = profile.DateOfBirth;

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ChangePasswordError"] = "Vui lòng kiểm tra lại thông tin.";
                TempData["ActiveTab"] = "sensy-change-password";

                // Lưu model để hiển thị lại
                TempData["ChangePasswordModel"] = JsonConvert.SerializeObject(model);

                return View("~/Views/Profile/Profile.cshtml");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.tblUsers.FindAsync(userId);

            if (user == null)
                return NotFound();

            if (user.Password != model.CurrentPassword)
            {
                TempData["ChangePasswordError"] = "Mật khẩu hiện tại không đúng.";
                TempData["ActiveTab"] = "sensy-change-password";

                TempData["ChangePasswordModel"] = JsonConvert.SerializeObject(model);

                return View("~/Views/Profile/Profile.cshtml");
            }

            user.Password = model.NewPassword;
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["ChangePasswordSuccess"] = "Đổi mật khẩu thành công.";
            TempData["ActiveTab"] = "sensy-change-password";

            return View("~/Views/Profile/Profile.cshtml");
        }


    }
}
