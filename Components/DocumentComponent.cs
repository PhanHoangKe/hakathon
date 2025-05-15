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
            
            // Lấy đánh giá của tài liệu
            var ratings = await _context.tblDocumentRatings
                .Where(r => r.DocumentID == documentId)
                .ToListAsync();
            
            ViewBag.AverageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
            ViewBag.RatingCount = ratings.Count;
            
            // Kiểm tra xem người dùng đã thêm vào yêu thích chưa
            // Giả sử userId được lấy từ session hoặc cookie
            int userId = 1; // Mẫu - Bạn cần thay thế với user ID thực tế từ authentication
            
            var isFavorite = await _context.tblFavorites
                .AnyAsync(f => f.DocumentID == documentId && f.UserID == userId);
                
            ViewBag.IsFavorite = isFavorite;
            
            return View(document);
        }
    }
}