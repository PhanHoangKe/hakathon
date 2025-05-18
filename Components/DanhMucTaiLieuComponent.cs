using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace hakathon.Components
{
    [ViewComponent(Name = "DanhMucTaiLieu")]
    public class DanhMucTaiLieuComponent : ViewComponent
    {
        private readonly DataContext _context;
        public DanhMucTaiLieuComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listOfMenu = (from m in _context.Categories
                              where (m.IsActive == true) 
                              select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listOfMenu));
        }
    }
}