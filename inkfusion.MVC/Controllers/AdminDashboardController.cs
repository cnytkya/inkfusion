using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inkfusion.MVC.Data;
using inkfusion.MVC.Models;
using inkfusion.MVC.Services;
using System.Security.Claims;

namespace inkfusion.MVC.Controllers
{
    [Route("admin")]
    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminDashboardController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminDashboardController(ApplicationDbContext context, ILogger<AdminDashboardController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsAdmin()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return role == UserRole.Admin.ToString();
        }

        private void CheckAdminAccess()
        {
            if (!IsAdmin())
            {
                throw new UnauthorizedAccessException("Bu sayfaya erişim izniniz yok.");
            }
        }

        private User? GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return _context.Users.Find(userId);
            }
            return null;
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            CheckAdminAccess();
            var user = GetCurrentUser();
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpGet("profile/edit")]
        public IActionResult EditProfile()
        {
            CheckAdminAccess();
            var user = GetCurrentUser();
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpPost("profile/edit")]
        public IActionResult EditProfile(User model)
        {
            CheckAdminAccess();
            var user = GetCurrentUser();
            if (user == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Bio = model.Bio;
                user.ProfileImageUrl = model.ProfileImageUrl;
                _context.SaveChanges();
                TempData["Success"] = "Profil başarıyla güncellendi!";
                return RedirectToAction("Profile");
            }
            return View(user);
        }

        [HttpGet("")]
        [HttpGet("dashboard")]
        public IActionResult Index()
        {
            CheckAdminAccess();

            var stats = new
            {
                TotalUsers = _context.Users.Count(u => u.IsActive),
                TotalCustomers = _context.Users.Count(u => u.Role == UserRole.Customer && u.IsActive),
                TotalAppointments = _context.Appointments.Count(),
                CompletedAppointments = _context.Appointments.Count(a => a.Status == AppointmentStatus.Completed),
                PendingAppointments = _context.Appointments.Count(a => a.Status == AppointmentStatus.Pending),
                TotalArtists = _context.Artists.Count(a => a.IsActive),
                TotalServices = _context.Services.Count(s => s.IsActive),
                ApprovedComments = _context.Comments.Count(c => c.IsApproved),
                PendingComments = _context.Comments.Count(c => !c.IsApproved),
                TotalGalleryItems = _context.GalleryItems.Count()
            };

            var recentAppointments = _context.Appointments
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Status,
                    UserName = $"{a.User.FirstName} {a.User.LastName}",
                    ArtistName = $"{a.Artist.FirstName} {a.Artist.LastName}"
                })
                .ToList();

            ViewBag.Stats = stats;
            ViewBag.RecentAppointments = recentAppointments;

            return View();
        }

        // Galleries Management
        [HttpGet("galleries")]
        public IActionResult Galleries()
        {
            CheckAdminAccess();
            var galleries = _context.GalleryItems
                .Include(g => g.Artist)
                .OrderByDescending(g => g.CreatedAt)
                .ToList();
            return View(galleries);
        }

        [HttpGet("galleries/create")]
        public IActionResult CreateGallery()
        {
            CheckAdminAccess();
            ViewBag.Artists = _context.Artists.Where(a => a.IsActive).ToList();
            ViewBag.Categories = _context.Categories.Where(c => c.IsActive).ToList();
            return View();
        }

        [HttpPost("galleries/create")]
        public IActionResult CreateGallery(GalleryItem model, IFormFile? imageFile)
        {
            CheckAdminAccess();

            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "gallery", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                model.ImageUrl = $"/img/gallery/uploads/{fileName}";
                ModelState.Remove("ImageUrl");
            }

            if (ModelState.IsValid && !string.IsNullOrEmpty(model.ImageUrl))
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.GalleryItems.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Galeri başarıyla oluşturuldu!";
                return RedirectToAction("Galleries");
            }
            ViewBag.Artists = _context.Artists.Where(a => a.IsActive).ToList();
            ViewBag.Categories = _context.Categories.Where(c => c.IsActive).ToList();
            return View(model);
        }

        [HttpGet("galleries/{id}/edit")]
        public IActionResult EditGallery(int id)
        {
            CheckAdminAccess();
            var gallery = _context.GalleryItems.Include(g => g.Artist).FirstOrDefault(g => g.Id == id);
            if (gallery == null)
                return NotFound();
            ViewBag.Artists = _context.Artists.Where(a => a.IsActive).ToList();
            ViewBag.Categories = _context.Categories.Where(c => c.IsActive).ToList();
            return View(gallery);
        }

        [HttpPost("galleries/{id}/edit")]
        public IActionResult EditGallery(int id, GalleryItem model, IFormFile? imageFile)
        {
            CheckAdminAccess();
            var gallery = _context.GalleryItems.Find(id);
            if (gallery == null)
                return NotFound();

            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "gallery", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                gallery.ImageUrl = $"/img/gallery/uploads/{fileName}";
            }

            if (ModelState.IsValid)
            {
                gallery.Title = model.Title;
                gallery.Description = model.Description;
                gallery.Category = model.Category;
                gallery.DisplayOrder = model.DisplayOrder;
                gallery.IsActive = model.IsActive;
                gallery.ArtistId = model.ArtistId;
                gallery.CategoryId = model.CategoryId;
                _context.SaveChanges();
                TempData["Success"] = "Galeri başarıyla güncellendi!";
                return RedirectToAction("Galleries");
            }
            ViewBag.Artists = _context.Artists.Where(a => a.IsActive).ToList();
            ViewBag.Categories = _context.Categories.Where(c => c.IsActive).ToList();
            return View(gallery);
        }

        [HttpPost("galleries/{id}/delete")]
        public IActionResult DeleteGallery(int id)
        {
            CheckAdminAccess();
            var gallery = _context.GalleryItems.Find(id);
            if (gallery != null)
            {
                _context.GalleryItems.Remove(gallery);
                _context.SaveChanges();
            }
            return RedirectToAction("Galleries");
        }

        // Artists Management
        [HttpGet("artists")]
        public IActionResult Artists()
        {
            CheckAdminAccess();
            var artists = _context.Artists
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
            return View(artists);
        }

        [HttpGet("artists/create")]
        public IActionResult CreateArtist()
        {
            CheckAdminAccess();
            return View();
        }

        [HttpPost("artists/create")]
        public IActionResult CreateArtist(Artist model)
        {
            CheckAdminAccess();
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.Artists.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Artists");
            }
            return View(model);
        }

        [HttpGet("artists/{id}/edit")]
        public IActionResult EditArtist(int id)
        {
            CheckAdminAccess();
            var artist = _context.Artists.Find(id);
            if (artist == null)
                return NotFound();
            return View(artist);
        }

        [HttpPost("artists/{id}/edit")]
        public IActionResult EditArtist(int id, Artist model)
        {
            CheckAdminAccess();
            var artist = _context.Artists.Find(id);
            if (artist == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                artist.FirstName = model.FirstName;
                artist.LastName = model.LastName;
                artist.Bio = model.Bio;
                artist.Specialization = model.Specialization;
                artist.ImageUrl = model.ImageUrl;
                artist.ExperienceYears = model.ExperienceYears;
                artist.InstagramUrl = model.InstagramUrl;
                artist.IsActive = model.IsActive;
                _context.SaveChanges();
                TempData["Success"] = "Sanatçı başarıyla güncellendi!";
                return RedirectToAction("Artists");
            }
            return View(artist);
        }

        [HttpPost("artists/{id}/delete")]
        [Route("artists/{id}/delete")]
        [HttpPost]
        public IActionResult DeleteArtist(int id)
        {
            CheckAdminAccess();
            var artist = _context.Artists.Find(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                _context.SaveChanges();
                TempData["Success"] = "Sanatçı başarıyla silindi!";
            }
            return RedirectToAction("Artists");
        }

        // Services Management
        [HttpGet("services")]
        public IActionResult Services()
        {
            CheckAdminAccess();
            var services = _context.Services
                .OrderBy(s => s.DisplayOrder)
                .ToList();
            return View(services);
        }

        [HttpGet("services/create")]
        public IActionResult CreateService()
        {
            CheckAdminAccess();
            return View();
        }

        [HttpPost("services/create")]
        public IActionResult CreateService(Service model)
        {
            CheckAdminAccess();
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.Services.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Services");
            }
            return View(model);
        }

        [HttpGet("services/{id}/edit")]
        public IActionResult EditService(int id)
        {
            CheckAdminAccess();
            var service = _context.Services.Find(id);
            if (service == null)
                return NotFound();
            return View(service);
        }

        [HttpPost("services/{id}/edit")]
        public IActionResult EditService(int id, Service model)
        {
            CheckAdminAccess();
            var service = _context.Services.Find(id);
            if (service == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                service.Name = model.Name;
                service.Description = model.Description;
                service.Price = model.Price;
                service.DurationMinutes = model.DurationMinutes;
                service.IconClass = model.IconClass;
                service.ImageUrl = model.ImageUrl;
                service.DisplayOrder = model.DisplayOrder;
                service.IsActive = model.IsActive;
                _context.SaveChanges();
                TempData["Success"] = "Hizmet başarıyla güncellendi!";
                return RedirectToAction("Services");
            }
            return View(service);
        }

        [Route("services/{id}/delete")]
        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            CheckAdminAccess();
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
                TempData["Success"] = "Hizmet başarıyla silindi!";
            }
            return RedirectToAction("Services");
        }

        // Appointments Management
        [HttpGet("appointments")]
        public IActionResult Appointments()
        {
            CheckAdminAccess();
            var appointments = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Artist)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
            return View(appointments);
        }

        [HttpPost("appointments/{id}/update-status")]
        public IActionResult UpdateAppointmentStatus(int id, AppointmentStatus status)
        {
            CheckAdminAccess();
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Status = status;
                if (status == AppointmentStatus.Completed)
                {
                    appointment.CompletedAt = DateTime.UtcNow;
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Appointments");
        }

        // Comments Management
        [HttpGet("comments")]
        public IActionResult Comments()
        {
            CheckAdminAccess();
            var comments = _context.Comments
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return View(comments);
        }

        [HttpPost("comments/{id}/approve")]
        public IActionResult ApproveComment(int id)
        {
            CheckAdminAccess();
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                comment.IsApproved = true;
                comment.ApprovedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
            return RedirectToAction("Comments");
        }

        [HttpPost("comments/{id}/delete")]
        public IActionResult DeleteComment(int id)
        {
            CheckAdminAccess();
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("Comments");
        }

        // Users Management
        [HttpGet("users")]
        public IActionResult Users()
        {
            CheckAdminAccess();
            var users = _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToList();
            return View(users);
        }

        [HttpPost("users/{id}/role")]
        public IActionResult ChangeUserRole(int id, UserRole role)
        {
            CheckAdminAccess();
            var user = _context.Users.Find(id);
            if (user != null && user.Id != 1) // Can't change admin user
            {
                user.Role = role;
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        [HttpPost("users/{id}/deactivate")]
        public IActionResult DeactivateUser(int id)
        {
            CheckAdminAccess();
            var user = _context.Users.Find(id);
            if (user != null && user.Id != 1)
            {
                user.IsActive = false;
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        [HttpPost("users/{id}/reactivate")]
        public IActionResult ReactivateUser(int id)
        {
            CheckAdminAccess();
            var user = _context.Users.Find(id);
            if (user != null && user.Id != 1)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        [HttpGet("users/create")]
        public IActionResult CreateUser()
        {
            CheckAdminAccess();
            return View();
        }

        [HttpPost("users/create")]
        public IActionResult CreateUser(SignupModel model)
        {
            CheckAdminAccess();

            if (ModelState.IsValid)
            {
                // E-posta kontrolü
                if (_context.Users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı");
                    return View(model);
                }

                // Kullanıcı adı kontrolü
                if (_context.Users.Any(u => u.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError("Username", "Bu kullanıcı adı zaten kayıtlı");
                    return View(model);
                }

                // Yeni kullanıcı oluştur
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = UserRole.Customer,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Users");
            }
            return View(model);
        }

        [HttpGet("users/{id}/edit")]
        public IActionResult EditUser(int id)
        {
            CheckAdminAccess();
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpPost("users/{id}/edit")]
        public IActionResult EditUser(int id, string FirstName, string LastName, string Role)
        {
            CheckAdminAccess();
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                ModelState.AddModelError("", "Ad ve Soyadı gereklidir");
                return View(user);
            }

            user.FirstName = FirstName;
            user.LastName = LastName;

            // Sadece Super Admin değilse rol değişikliği yapılabilir
            if (user.Id != 1 && !string.IsNullOrWhiteSpace(Role))
            {
                if (Enum.TryParse<UserRole>(Role, out var roleEnum))
                {
                    user.Role = roleEnum;
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Kullanıcı başarıyla güncellendi!";
            return RedirectToAction("Users");
        }

        [HttpPost("users/{id}/delete")]
        public IActionResult DeleteUser(int id)
        {
            CheckAdminAccess();
            var user = _context.Users.Find(id);
            if (user != null && user.Id != 1)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
            }
            return RedirectToAction("Users");
        }

        // Categories Management
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            CheckAdminAccess();
            var categories = _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToList();
            return View(categories);
        }

        [HttpGet("categories/create")]
        public IActionResult CreateCategory()
        {
            CheckAdminAccess();
            return View();
        }

        [HttpPost("categories/create")]
        public IActionResult CreateCategory(Category model)
        {
            CheckAdminAccess();
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.Categories.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Kategori başarıyla oluşturuldu!";
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        [HttpGet("categories/{id}/edit")]
        public IActionResult EditCategory(int id)
        {
            CheckAdminAccess();
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        [HttpPost("categories/{id}/edit")]
        public IActionResult EditCategory(int id, Category model)
        {
            CheckAdminAccess();
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                category.Name = model.Name;
                category.Description = model.Description;
                category.Icon = model.Icon;
                category.DisplayOrder = model.DisplayOrder;
                category.IsActive = model.IsActive;
                _context.SaveChanges();
                TempData["Success"] = "Kategori başarıyla güncellendi!";
                return RedirectToAction("Categories");
            }
            return View(category);
        }

        [HttpPost("categories/{id}/delete")]
        public IActionResult DeleteCategory(int id)
        {
            CheckAdminAccess();
            var category = _context.Categories.Find(id);
            if (category != null && id > 6) // Can't delete default categories (1-6)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["Success"] = "Kategori başarıyla silindi!";
            }
            return RedirectToAction("Categories");
        }

        [HttpGet("api/stats")]
        [AllowAnonymous]
        public IActionResult GetStats()
        {
            var stats = new
            {
                totalImages = _context.GalleryItems.Count(),
                activeImages = _context.GalleryItems.Count(g => g.IsActive),
                totalCategories = _context.Categories.Count(),
                totalArtists = _context.Artists.Count(),
                totalUsers = _context.Users.Count(),
                totalAppointments = _context.Appointments.Count(),
                totalComments = _context.Comments.Count(),
                imagesByCategory = _context.GalleryItems
                    .GroupBy(g => g.CategoryId)
                    .Select(g => new { categoryId = g.Key, count = g.Count() })
                    .ToList()
            };
            return Ok(stats);
        }

        [HttpPost("import-images")]
        public async Task<IActionResult> ImportImages()
        {
            CheckAdminAccess();
            try
            {
                var importService = new ImageImportService(_context, _webHostEnvironment.WebRootPath);
                await importService.ImportImagesFromDirectoriesAsync();
                TempData["Success"] = "Resimler başarıyla DB'ye yazıldı!";
                return RedirectToAction("Galleries");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return RedirectToAction("Galleries");
            }
        }
    }
}
