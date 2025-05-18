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
	[ViewComponent(Name = "Contact")]
	public class ContactComponent : ViewComponent
	{
		private readonly DataContext _context;
		public ContactComponent(DataContext context)
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


			return View("Default");
		}
	}
}
