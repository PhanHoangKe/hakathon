using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Models;
using Microsoft.AspNetCore.Mvc;

namespace hakathon.Components
{
    [ViewComponent(Name = "Image")]
    public class ImageComponent:ViewComponent
{
 private readonly DataContext _context;
        public ImageComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listOfMenu = (from m in _context.carouselImages
                              where (m.IsActive == true)
                              select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listOfMenu));
        }
}
}