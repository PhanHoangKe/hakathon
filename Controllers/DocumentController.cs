using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hakathon.Models;

namespace hakathon.Controllers
{
    public class DocumentController : Controller
    {
        private readonly DataContext _context;

        public DocumentController(DataContext context)
        {
            _context = context;
        }

        // Hiển thị trang chủ với danh sách tài liệu mới nhất
        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            // Trả về view và Component Category sẽ xử lý phần còn lại
            return View();
        }

        // Xem chi tiết tài liệu
        public async Task<IActionResult> Details(int id)
        {
            var document = await _context.tblDocuments
                .Where(d => d.DocumentID == id && d.IsApproved && d.IsActive)
                .Include(d => d.DocumentAuthor)
                    .ThenInclude(da => da.Author)
                .Include(d => d.Category)
                .Include(d => d.Publisher)
                .FirstOrDefaultAsync();

            if (document == null)
            {
                return NotFound();
            }

            // Tăng số lượt xem
            document.ViewCount++;
            _context.Update(document);
            await _context.SaveChangesAsync();

            // Lấy đánh giá của tài liệu
            var ratings = await _context.tblDocumentRatings
                .Where(r => r.DocumentID == id)
                .ToListAsync();

            ViewBag.AverageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
            ViewBag.RatingCount = ratings.Count;
            
            // Kiểm tra xem người dùng đã thêm vào yêu thích chưa
            // Giả sử userId được lấy từ session hoặc cookie
            int userId = 1; // Mẫu - Bạn cần thay thế với user ID thực tế từ authentication
            
            var isFavorite = await _context.tblFavorites
                .AnyAsync(f => f.DocumentID == id && f.UserID == userId);
                
            ViewBag.IsFavorite = isFavorite;
            ViewBag.DocumentId = id;

            return View(document);
        }
        
        // Xem tài liệu trực tiếp
        public async Task<IActionResult> View(int id)
        {
            var document = await _context.tblDocuments
                .FirstOrDefaultAsync(d => d.DocumentID == id && d.IsApproved && d.IsActive);

            if (document == null)
            {
                return NotFound();
            }

            // Tăng số lượt xem
            document.ViewCount++;
            _context.Update(document);
            await _context.SaveChangesAsync();

            // Trả về view với URL của file PDF
            ViewBag.PdfUrl = document.FilePDFPath;
            ViewBag.DocumentTitle = document.Title;
            
            return View();
        }

        // API để tải tài liệu
        [HttpPost]
        public async Task<IActionResult> Download(int id, string type = "pdf")
        {
            var document = await _context.tblDocuments
                .FirstOrDefaultAsync(d => d.DocumentID == id && d.IsApproved && d.IsActive);

            if (document == null)
            {
                return NotFound();
            }

            // Tăng số lượt tải
            document.DownloadCount++;
            _context.Update(document);
            await _context.SaveChangesAsync();

            string filePath;
            string contentType;
            string fileName;

            if (type == "original" && !string.IsNullOrEmpty(document.FileOriginalPath))
            {
                filePath = document.FileOriginalPath;
                contentType = GetContentType(document.FileType);
                fileName = Path.GetFileName(document.FileOriginalPath);
            }
            else
            {
                filePath = document.FilePDFPath;
                contentType = "application/pdf";
                fileName = Path.GetFileName(document.FilePDFPath);
            }

            // Trả về file
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType, fileName);
            }

            return NotFound();
        }

        private string GetContentType(string fileType)
        {
            switch (fileType?.ToLower())
            {
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "doc":
                    return "application/msword";
                case "xls":
                    return "application/vnd.ms-excel";
                case "xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "ppt":
                    return "application/vnd.ms-powerpoint";
                case "pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "txt":
                    return "text/plain";
                default:
                    return "application/octet-stream";
            }
        }

        // API để thêm đánh giá
        [HttpPost]
        public async Task<IActionResult> AddRating(int documentId, int rating, string comment)
        {
            // Giả sử userId được lấy từ session hoặc cookie
            int userId = 1; // Mẫu

            var existingRating = await _context.tblDocumentRatings
                .FirstOrDefaultAsync(r => r.DocumentID == documentId && r.UserID == userId);

            if (existingRating != null)
            {
                existingRating.Rating = rating;
                existingRating.Comment = comment;
                existingRating.RatingDate = DateTime.Now;
                _context.Update(existingRating);
            }
            else
            {
                _context.tblDocumentRatings.Add(new tblDocumentRatings
                {
                    DocumentID = documentId,
                    UserID = userId,
                    Rating = rating,
                    Comment = comment,
                    RatingDate = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            // Tính điểm trung bình mới
            var averageRating = await _context.tblDocumentRatings
                .Where(r => r.DocumentID == documentId)
                .AverageAsync(r => r.Rating);

            return Json(new { success = true, averageRating });
        }

        // API để thêm/xóa yêu thích
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite([FromBody] FavoriteRequest request)
        {
            int documentId = request.DocumentId;
            
            // Giả sử userId được lấy từ session hoặc cookie
            int userId = 1; // Mẫu - Cần thay thế bằng userId thực từ authentication

            var favorite = await _context.tblFavorites
                .FirstOrDefaultAsync(f => f.DocumentID == documentId && f.UserID == userId);

            bool isFavorite;

            if (favorite != null)
            {
                _context.tblFavorites.Remove(favorite);
                isFavorite = false;
            }
            else
            {
                _context.tblFavorites.Add(new tblFavorites
                {
                    DocumentID = documentId,
                    UserID = userId,
                    DateAdded = DateTime.Now
                });
                isFavorite = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, isFavorite });
        }
        
        // Class để deserialize yêu cầu từ AJAX
        public class FavoriteRequest
        {
            public int DocumentId { get; set; }
        }

        [HttpGet]
    public IActionResult GetPdfPreview(int id)
    {
        try {
            // Log để debug
            System.Console.WriteLine($"GetPdfPreview called for document ID: {id}");
            
            // Path tới file PDF mẫu trong wwwroot
            string hardcodedPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents", "ke.pdf");
            
            // Kiểm tra file tồn tại
            if (System.IO.File.Exists(hardcodedPath))
            {
                System.Console.WriteLine($"Found PDF file at: {hardcodedPath}");
                var fileBytes = System.IO.File.ReadAllBytes(hardcodedPath);
                return File(fileBytes, "application/pdf");
            }
            else
            {
                System.Console.WriteLine($"PDF file not found at: {hardcodedPath}");
                
                // Tạo thư mục nếu chưa tồn tại
                string directoryPath = Path.GetDirectoryName(hardcodedPath);
                if (!Directory.Exists(directoryPath))
                {
                    System.Console.WriteLine($"Creating directory: {directoryPath}");
                    Directory.CreateDirectory(directoryPath);
                }
                
                // Trả về một file PDF rỗng hoặc mẫu nếu file không tồn tại
                byte[] emptyPdfBytes = GenerateEmptyPdf();
                return File(emptyPdfBytes, "application/pdf");
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error in GetPdfPreview: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    // Hàm tạo PDF rỗng đơn giản (cần thêm thư viện iTextSharp hoặc tương tự trong dự án thực tế)
    private byte[] GenerateEmptyPdf()
    {
        // Đây là một PDF rỗng cơ bản, minimal valid PDF
        string minimalPdf = "%PDF-1.4\n" +
                           "1 0 obj<</Type/Catalog/Pages 2 0 R>>endobj\n" +
                           "2 0 obj<</Type/Pages/Kids[3 0 R]/Count 1>>endobj\n" +
                           "3 0 obj<</Type/Page/MediaBox[0 0 595 842]/Parent 2 0 R/Resources<<>>>>endobj\n" +
                           "xref\n" +
                           "0 4\n" +
                           "0000000000 65535 f\n" +
                           "0000000009 00000 n\n" +
                           "0000000056 00000 n\n" +
                           "0000000111 00000 n\n" +
                           "trailer<</Size 4/Root 1 0 R>>\n" +
                           "startxref\n" +
                           "188\n" +
                           "%%EOF";
        
        return System.Text.Encoding.ASCII.GetBytes(minimalPdf);
    }

        [HttpPost]
public async Task<IActionResult> TrackViewProgress(int documentId, int currentPage, int viewDuration)
{
    // Get current user ID - in a real app, this would come from authentication
    int userId = 1; // Example user ID
    
    // Check if there's an existing view history for this user and document
    var viewHistory = await _context.tblViewHistories
        .FirstOrDefaultAsync(vh => vh.UserID == userId && vh.DocumentID == documentId);
        
    if (viewHistory == null)
    {
        // Create new view history
        viewHistory = new tblViewHistory
        {
            UserID = userId,
            DocumentID = documentId,
            ViewDate = DateTime.Now,
            LastPageViewed = currentPage,
            ViewDuration = viewDuration
        };
        
        _context.tblViewHistories.Add(viewHistory);
    }
    else
    {
        // Update existing view history
        viewHistory.LastPageViewed = currentPage;
        viewHistory.ViewDuration += viewDuration;
        viewHistory.ViewDate = DateTime.Now; // Update to most recent view
        
        _context.tblViewHistories.Update(viewHistory);
    }
    
    await _context.SaveChangesAsync();
    
    return Json(new { success = true });
}
    }
}