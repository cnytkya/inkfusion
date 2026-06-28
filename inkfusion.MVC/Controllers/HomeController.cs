using System.Diagnostics;
using inkfusion.MVC.Data;
using inkfusion.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace inkfusion.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var artists = _context.Artists.Where(a => a.IsActive).ToList();
            var services = _context.Services.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToList();
            var galleryItems = _context.GalleryItems.Where(g => g.IsActive).OrderBy(g => g.DisplayOrder).Take(12).ToList();
            ViewBag.Artists = artists;
            ViewBag.Services = services;
            ViewBag.GalleryItems = galleryItems;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "admin@inkfusion.com" && model.Password == "admin123")
                {
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("", "Geçersiz e-posta veya şifre");
            }
            return View(model);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
