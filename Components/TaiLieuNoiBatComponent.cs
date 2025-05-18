using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hakathon.Components
{
    [ViewComponent(Name = "TaiLieuNoiBat")]
    public class TaiLieuNoiBatComponent:ViewComponent
    {
        private readonly DataContext _context;
        public TaiLieuNoiBatComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách nổi bật theo lượt xem + tải + thích (ưu tiên nhiều nhất)
            var listOfMenu = _context.viewDocumentAuthors
                                .Where(m => m.IsActive)

                                .OrderByDescending(m => m.ViewCount+ m.DownloadCount  )
                                .Take(3) // Giới hạn 9 tài liệu nổi bật (3x3)
                                .ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", listOfMenu));
    

}

    }
}