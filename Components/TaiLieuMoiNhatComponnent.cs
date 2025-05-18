using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hakathon.Components
{
    [ViewComponent(Name = "TaiLieuMoiNhat")]
    public class TaiLieuMoiNhatComponnent : ViewComponent
    {
        private readonly DataContext _context;
        public TaiLieuMoiNhatComponnent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listOfMenu = _context.viewDocumentAuthors
                                     .Where(m => m.IsActive)
                                     .OrderByDescending(m => m.CreatedDate) // Mới nhất lên đầu
                                     .ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", listOfMenu));




        }
    }
}