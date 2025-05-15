using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;

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
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await Task.Run(() => _context.viewCategoryMenus
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToList());       
            return View(categories);
        }
    }
}