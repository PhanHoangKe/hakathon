using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hakathon.Components
{
    [ViewComponent(Name = "Document")]
    public class DocumentComponent : ViewComponent
    {
        private readonly DataContext _context;

        public DocumentComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int documentId)
        {
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                string userIdStr = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdStr, out int parsedId))
                {
                    userId = parsedId;
                }
            }

            var document = await _context.tblDocuments
                .Where(d => d.DocumentID == documentId && d.IsApproved && d.IsActive)
                .Include(d => d.DocumentAuthor)
                    .ThenInclude(da => da.Author)
                .Include(d => d.Category)
                .Include(d => d.Publisher)
                .FirstOrDefaultAsync();

            if (document == null)
            {
                return View("Error");
            }

            var ratings = await _context.tblDocumentRatings
                .Where(r => r.DocumentID == documentId)
                .ToListAsync();

            ViewBag.AverageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
            ViewBag.RatingCount = ratings.Count;

            if (userId.HasValue)
            {
                var isFavorite = await _context.tblFavorites
                    .AnyAsync(f => f.DocumentID == documentId && f.UserID == userId.Value);

                ViewBag.IsFavorite = isFavorite;
            }
            else
            {
                ViewBag.IsFavorite = false;
            }

            return View(document);
        }
    }
}
