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
    public class FavoriteDocumentIds : Controller
    {
        private readonly DataContext _context;
        private readonly IAntiforgery _antiforgery;

        public FavoriteDocumentIds(DataContext context, IAntiforgery antiforgery)
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
                MenuID = document.MenuID 
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

        [HttpGet]
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
                await _context.SaveChangesAsync();
                Console.WriteLine($"[INFO] Download count for document {id} incremented to {document.DownloadCount}");

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