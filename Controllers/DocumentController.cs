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
    public class DocumentController : Controller
    {
        private readonly DataContext _context;
	
		public DocumentController(DataContext context)
        {
            _context = context;
            
        }

        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            
            ViewBag.DocumentId = id;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> IncrementViewCount(int id)
        {
            try
            {
                var document = await _context.tblDocuments
                    .FirstOrDefaultAsync(d => d.DocumentID == id && d.IsApproved && d.IsActive);

                if (document == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy tài liệu" });
                }

                document.ViewCount++;
                _context.Update(document);
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    viewCount = document.ViewCount,
                    message = "Tăng lượt xem thành công" 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IncrementViewCount: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi server" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Download(int id, string type = "pdf")
        {
            try
            {
                var document = await _context.tblDocuments
                    .FirstOrDefaultAsync(d => d.DocumentID == id && d.IsApproved && d.IsActive);
        
                if (document == null)
                {
                    Console.WriteLine($"[ERROR] Document with ID {id} not found or not approved/active");
                    return NotFound(new { success = false, message = "Không tìm thấy tài liệu" });
                }

				var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
				if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
				{
					return Unauthorized(new { success = false, message = "Người dùng chưa đăng nhập" });
				}

				var userProfile = await _context.tblUserProfiles.FirstOrDefaultAsync(p => p.UserID == userId);
				if (userProfile == null)
				{
					return NotFound(new { success = false, message = "Không tìm thấy hồ sơ người dùng" });
				}

				int requiredPoints = document.Point ?? 0;

				if ((userProfile.Point ?? 0) < requiredPoints)
				{
					return BadRequest(new { success = false, message = $"Bạn cần {requiredPoints} điểm để tải tài liệu này" });
				}

				
				userProfile.Point = (userProfile.Point ?? 0) - requiredPoints;

				string filePath;
                string contentType;
                string fileName;
        
                if (type == "original" && !string.IsNullOrEmpty(document.FileOriginalPath))
                {
                    filePath = document.FileOriginalPath;
                    contentType = GetContentType(document.FileType);
                    fileName = Path.GetFileName(document.FileOriginalPath);
                    Console.WriteLine($"[INFO] Serving original file: {filePath}");
                }
                else
                {
                    filePath = document.FilePDFPath;
                    contentType = "application/pdf";
                    fileName = Path.GetFileName(document.FilePDFPath);
                    Console.WriteLine($"[INFO] Serving PDF file: {filePath}");
                }

                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"[ERROR] File not found at path: {filePath}");
                    return NotFound(new { success = false, message = "Không tìm thấy file tài liệu" });
                }

				document.DownloadCount++;
				_context.Update(document);
				_context.Update(userProfile);
				await _context.SaveChangesAsync();



				var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in Download: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi khi tải xuống tài liệu" });
            }
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

        [HttpPost]
        public async Task<IActionResult> AddRating(int documentId, int rating, string comment)
        {
            int? userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập để đánh giá" });
            }

            var existingRating = await _context.tblDocumentRatings
                .FirstOrDefaultAsync(r => r.DocumentID == documentId && r.UserID == userId.Value);

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
                    UserID = userId.Value,
                    Rating = rating,
                    Comment = comment,
                    RatingDate = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            var averageRating = await _context.tblDocumentRatings
                .Where(r => r.DocumentID == documentId)
                .AverageAsync(r => r.Rating);

            return Json(new { success = true, averageRating });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite([FromBody] FavoriteRequest request)
        {
            int documentId = request.DocumentId;
        
            int? userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập để thêm vào yêu thích." });
            }
        
            var document = await _context.tblDocuments.FindAsync(documentId);
            if (document == null)
            {
                return NotFound(new { success = false, message = "Tài liệu không tồn tại." });
            }
        
            var favorite = await _context.tblFavorites
                .FirstOrDefaultAsync(f => f.DocumentID == documentId && f.UserID == userId.Value);
        
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
                    UserID = userId.Value,
                    DateAdded = DateTime.Now,
                    MenuID = document.MenuID 
                });
                isFavorite = true;
            }
        
            await _context.SaveChangesAsync();
        
            return Json(new { success = true, isFavorite });
        }

        public class FavoriteRequest
        {
            public int DocumentId { get; set; }
        }

        [HttpGet]
        public IActionResult GetPdfPreview(int id)
        {
            try
            {
                Console.WriteLine($"[INFO] GetPdfPreview called for document ID: {id}");

                var document = _context.tblDocuments
                    .FirstOrDefault(d => d.DocumentID == id && d.IsApproved && d.IsActive);

                string pdfPath;

                if (document != null && !string.IsNullOrEmpty(document.FilePDFPath) && System.IO.File.Exists(document.FilePDFPath))
                {
                    pdfPath = document.FilePDFPath;
                }
                else
                {
                    pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents", "ke.pdf");
                }

                if (System.IO.File.Exists(pdfPath))
                {
                    byte[] limitedPdfBytes = CreateLimitedPdf(pdfPath, 3);
                    return File(limitedPdfBytes, "application/pdf");
                }
                else
                {
                    Console.WriteLine($"[WARN] PDF file not found at: {pdfPath}");

                    string directoryPath = Path.GetDirectoryName(pdfPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Console.WriteLine($"[INFO] Creating directory: {directoryPath}");
                        Directory.CreateDirectory(directoryPath);
                    }

                    return File(GenerateEmptyPdf(), "application/pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetPdfPreview: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private byte[] CreateLimitedPdf(string sourcePdfPath, int maxPages)
        {
            try
            {
                using var outputStream = new MemoryStream();
                using var sourcePdf = new PdfDocument(new PdfReader(sourcePdfPath));
                using var destinationPdf = new PdfDocument(new PdfWriter(outputStream));
        
                int totalPages = sourcePdf.GetNumberOfPages();
                int pagesToCopy = Math.Min(totalPages, maxPages);
        
                Console.WriteLine($"[INFO] Source PDF has {totalPages} pages, copying {pagesToCopy}");
        
                sourcePdf.CopyPagesTo(1, pagesToCopy, destinationPdf);
        
                destinationPdf.Close(); 
        
                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error creating limited PDF: {ex.Message}");
        
                try
                {
                    return System.IO.File.ReadAllBytes(sourcePdfPath);
                }
                catch
                {
                    return GenerateEmptyPdf();
                }
            }
        }
        
        private byte[] GenerateEmptyPdf()
        {
            using var memoryStream = new MemoryStream();
            using var writer = new PdfWriter(memoryStream);
            using var pdfDoc = new PdfDocument(writer);
        
            pdfDoc.AddNewPage(); 
        
            return memoryStream.ToArray();
        }

        public class TrackViewRequest
        {
            public int DocumentId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> TrackInitialView([FromBody] TrackViewRequest request)
        {
            int documentId = request.DocumentId;
            int? userId = GetCurrentUserId(); 
            if (!userId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập." });
            }


            var existingView = await _context.tblViewHistories
                .FirstOrDefaultAsync(vh => vh.UserID == userId.Value && vh.DocumentID == documentId);

            if (existingView != null)
            {
				existingView.ViewDate = DateTime.Now;

				await _context.SaveChangesAsync();
				return Json(new { success = true, message = "Đã cập nhật thời gian xem lại." });
			}
            else
            {
                var viewHistory = new tblViewHistory
                {
                    UserID = userId.Value,
                    DocumentID = documentId,
                    ViewDate = DateTime.Now,
                    LastPageViewed = 1,
                    ViewDuration = 0
                };

                _context.tblViewHistories.Add(viewHistory);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TrackDownload([FromBody] TrackDownloadRequest request)
        {
            int documentId = request.DocumentId;
            int? userId = GetCurrentUserId();

            if (!userId.HasValue)
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập." });

            var history = new tblDownloadHistory
            {
                UserID = userId.Value,
                DocumentID = documentId,
                DownloadDate = DateTime.Now
            };

            _context.tblDownloadHistories.Add(history);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Ghi nhận tải xuống thành công." });
        }

        public class TrackDownloadRequest
        {
            public int DocumentId { get; set; }
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

        [HttpPost]
        public async Task<IActionResult> UpdateViewDuration([FromBody] ViewDurationUpdateModel model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized();
            }

            var latestView = await _context.tblViewHistories
                .Where(v => v.UserID == userId && v.DocumentID == model.DocumentId)
                .OrderByDescending(v => v.ViewDate)
                .FirstOrDefaultAsync();

            if (latestView == null)
                return NotFound("Không tìm thấy lịch sử xem.");

            latestView.ViewDuration = (latestView.ViewDuration ?? 0) + model.SecondsViewed;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        public class ViewDurationUpdateModel
        {
            public int DocumentId { get; set; }
            public int SecondsViewed { get; set; }
        }

		
	}
}