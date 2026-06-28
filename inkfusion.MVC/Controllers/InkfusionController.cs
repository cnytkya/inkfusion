using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inkfusion.MVC.Data;

namespace inkfusion.MVC.Controllers
{
    public class InkfusionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InkfusionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult gallery()
        {
            var galleries = _context.GalleryItems
                .Where(g => g.IsActive)
                .Include(g => g.Artist)
                .Include(g => g.CategoryNavigation)
                .OrderByDescending(g => g.DisplayOrder)
                .ThenByDescending(g => g.CreatedAt)
                .ToList();
            return View(galleries);
        }
    }
}
