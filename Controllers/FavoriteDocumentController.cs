using System;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;

namespace hakathon.Controllers
{
    public class FavoriteDocumentController : Controller
    {
        private readonly DataContext _context;
        private readonly IAntiforgery _antiforgery;

        public FavoriteDocumentController(DataContext context, IAntiforgery antiforgery)
        {
            _context = context;
            _antiforgery = antiforgery;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int documentId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    isAuthenticated = false,
                    message = "Vui lòng đăng nhập để sử dụng chức năng này."
                });
            }

        try
        {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var document = await _context.tblDocuments.FindAsync(documentId);
        if (document == null)
        {
            return Json(new { success = false, message = "Tài liệu không tồn tại." });
        }

        var existingFavorite = await _context.tblFavorites
            .FirstOrDefaultAsync(f => f.UserID == userId && f.DocumentID == documentId);

        bool isFavorite;

        if (existingFavorite != null)
        {
            _context.tblFavorites.Remove(existingFavorite);
            isFavorite = false;
        }
        else
        {
            var newFavorite = new tblFavorites
            {
                UserID = userId,
                DocumentID = documentId,
                DateAdded = DateTime.Now,
                
            };
            _context.tblFavorites.Add(newFavorite);
            isFavorite = true;
        }

        await _context.SaveChangesAsync();

        return Json(new { success = true, isFavorite });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ToggleFavorite: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var favorites = await _context.tblFavorites
                .Include(f => f.Document)
                    .ThenInclude(d => d.Category)
                .Include(f => f.Document)
                    .ThenInclude(d => d.DocumentAuthor)
                        .ThenInclude(da => da.Author)
                .Where(f => f.UserID == userId)
                .OrderByDescending(f => f.DateAdded)
                .ToListAsync();

            return View(favorites);
        }
    }
}