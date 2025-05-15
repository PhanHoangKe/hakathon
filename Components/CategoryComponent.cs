using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hakathon.Components
{
    [ViewComponent(Name = "Category")]
    public class CategoryComponent : ViewComponent
    {
        private readonly DataContext _context;

        public CategoryComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1, int pageSize = 12)
        {
            // Lấy danh sách danh mục
            var categories = await _context.viewCategoryMenus
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            // Lấy danh sách tài liệu đã được duyệt và active
            var documents = await _context.tblDocuments
                .Where(d => d.IsApproved && d.IsActive)
                .Include(d => d.DocumentAuthor)
                    .ThenInclude(da => da.Author)
                .Include(d => d.Category)
                .OrderByDescending(d => d.UploadDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Lấy tổng số tài liệu
            var totalDocuments = await _context.tblDocuments
                .Where(d => d.IsApproved && d.IsActive)
                .CountAsync();

            // Lấy danh sách tác giả
            var authors = await _context.tblAuthors
                .Where(a => a.IsActive)
                .OrderBy(a => a.AuthorName)
                .ToListAsync();

            // Tính số lượng tài liệu theo danh mục
            var categoryCounts = await _context.tblDocuments
                .Where(d => d.IsApproved && d.IsActive)
                .GroupBy(d => d.CategoryID)
                .Select(g => new { CategoryID = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CategoryID, x => x.Count);

            // Tính số lượng tài liệu theo tác giả
            var authorCounts = await _context.tblDocumentAuthors
                .GroupBy(da => da.AuthorID)
                .Select(g => new { AuthorID = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.AuthorID, x => x.Count);

            // Tính điểm đánh giá trung bình cho mỗi tài liệu
            var ratings = await _context.tblDocumentRatings
                .GroupBy(r => r.DocumentID)
                .Select(g => new { DocumentID = g.Key, Rating = (decimal)g.Average(r => r.Rating) })
                .ToDictionaryAsync(x => x.DocumentID, x => x.Rating);

            // Thiết lập ViewBag
            ViewBag.Documents = documents;
            ViewBag.Authors = authors;
            ViewBag.CategoryCounts = categoryCounts;
            ViewBag.AuthorCounts = authorCounts;
            ViewBag.Ratings = ratings;
            ViewBag.CurrentCount = documents.Count;
            ViewBag.TotalCount = totalDocuments;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalDocuments / (double)pageSize);

            // Tạo danh sách filter đang hoạt động (ví dụ)
            ViewBag.ActiveFilters = new List<dynamic> {
                new { Id = 1, Name = "Công nghệ thông tin", Type = "category" },
                new { Id = 1, Name = "Tiếng Việt", Type = "language" }
            };

            return View(categories);
        }
    }
}