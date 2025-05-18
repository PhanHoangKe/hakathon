using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace hakathon.Components
{
	[ViewComponent(Name = "Profile")]
	public class ProfileComponent : ViewComponent
	{
		private readonly DataContext _context;
		public ProfileComponent(DataContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			string? userIdStr = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
			{
				return Content("Người dùng chưa đăng nhập hoặc ID không hợp lệ.");
			}

			var profile = await _context.tblUserProfiles
				.FirstOrDefaultAsync(p => p.UserID == userId);

			var viewHistories = await _context.tblViewHistories
		.Where(vh => vh.UserID == userId)
		.Include(vh => vh.Document)
		.OrderByDescending(vh => vh.ViewDate)
		.Take(10)
		.ToListAsync();

			var downloadHistories = await _context.tblDownloadHistories
	.Where(dh => dh.UserID == userId)
	.Include(dh => dh.Document)
	.OrderByDescending(dh => dh.DownloadDate)
	.Take(10)
	.ToListAsync();

			var ratingHistories = await _context.tblDocumentRatings
	.Where(dh => dh.UserID == userId)
	.Include(dh => dh.Document)
	.OrderByDescending(dh => dh.RatingDate)
	.Take(10)
	.ToListAsync();

			var transactionHistories = await _context.tblTransactionHistories
				.Where(th => th.UserID == userId)
				.OrderByDescending(th => th.ActionDate)
				.Take(10)
				.ToListAsync();

			var documentcategories = await _context.tblCategories
	.OrderBy(c => c.CategoryName) 
	.ToListAsync();

			if (profile == null)
			{
				ViewBag.ViewHistories = viewHistories;
				ViewBag.Categories = documentcategories;
				return View("Default", (tblUserProfiles?)null);
			}

			ViewBag.FullName = profile.FullName;
			ViewBag.Bio = profile.Bio;
			ViewBag.Phone = profile.Phone;
			ViewBag.Address = profile.Address;
			ViewBag.DateOfBirth = profile.DateOfBirth?.ToString("dd/MM/yyyy");
			ViewBag.LastLoginDate = profile.LastLoginDate?.ToString("dd/MM/yyyy HH:mm");
			ViewBag.ProfilePicture = profile.ProfilePicture;
			ViewBag.ViewHistories = viewHistories;
			ViewBag.DownloadHistories = downloadHistories;
			ViewBag.RatingHistories = ratingHistories;
			ViewBag.TransactionHistories = transactionHistories;
			ViewBag.Categories = documentcategories;

			return View("Default", profile);
		}

	}
}
