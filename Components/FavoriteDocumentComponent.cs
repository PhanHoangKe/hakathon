using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace hakathon.Components
{
    [ViewComponent(Name = "FavoriteDocument")]
    public class FavoriteDocumentViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        
        public FavoriteDocumentViewComponent(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(int? userId = null)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
        
            if (userId == null && claimsPrincipal?.Identity?.IsAuthenticated == true)
            {
                userId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
        
            var favorites = await _context.viewFavoriteDocumentMenus
                .Where(f => f.UserID == userId) // 👈 Lọc dữ liệu theo người dùng
                .Include(f => f.Document)
                    .ThenInclude(d => d.Category)
                .Include(f => f.Document)
                    .ThenInclude(d => d.DocumentAuthor)
                        .ThenInclude(da => da.Author)
                .Include(f => f.User)
                .OrderByDescending(f => f.DateAdded)
                .ToListAsync();
        
            return View(favorites);
        }
    }
}