using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hakathon.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace hakathon.Controllers
{

	[Route("api/[controller]/[action]")]
	public class ContactController : Controller
	{
		private readonly DataContext _context;

		public ContactController(DataContext context)
		{
			_context = context;
		}

		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SubmitContact(tblContactMessages model)
		{
            int? userId = GetCurrentUserId();

            if (!userId.HasValue)
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập." });

            if (ModelState.IsValid)
			{
                model.UserID = userId;
                model.MessageDate = DateTime.Now;
				model.IsRead = false;
				model.IsReplied = false;

				_context.tblContactMessages.Add(model);
				await _context.SaveChangesAsync();

				TempData["Success"] = "Phản hồi của bạn đã được gửi thành công!";

				return RedirectToAction("Contact", "Home"); 
			}

			TempData["Error"] = "Gửi phản hồi thất bại. Vui lòng kiểm tra lại.";
			return View("Contact", model); 
		}

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}