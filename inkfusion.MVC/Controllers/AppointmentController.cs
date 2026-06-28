using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inkfusion.MVC.Data;
using inkfusion.MVC.Models;
using System.Security.Claims;

namespace inkfusion.MVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(ApplicationDbContext context, ILogger<AppointmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Book()
        {
            var artists = _context.Artists.Where(a => a.IsActive).OrderBy(a => a.FirstName).ToList();
            var services = _context.Services.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToList();

            _logger.LogInformation($"Book GET - Artists: {string.Join(", ", artists.Select(a => $"{a.Id}:{a.FirstName}"))}");
            _logger.LogInformation($"Book GET - Services: {string.Join(", ", services.Select(s => $"{s.Id}:{s.Name}"))}");

            ViewBag.Artists = artists;
            ViewBag.Services = services;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    ViewBag.CurrentUser = user;
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] AppointmentViewModel model)
        {
            _logger.LogInformation($"RAW REQUEST - ArtistId: {model.ArtistId} (type: {model.ArtistId.GetType().Name}), ServiceId: {model.ServiceId} (type: {model.ServiceId.GetType().Name})");
            _logger.LogInformation($"Model state valid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Form validation failed", errors = errors });
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int? userId = null;

                if (int.TryParse(userIdClaim, out int parsedUserId))
                {
                    userId = parsedUserId;
                }

                var allServices = await _context.Services.ToListAsync();
                _logger.LogInformation($"Database'de {allServices.Count} hizmet var: {string.Join(", ", allServices.Select(s => $"{s.Id}:{s.Name}"))}");

                var service = await _context.Services.FindAsync(model.ServiceId);
                if (service == null)
                {
                    _logger.LogWarning($"Service bulunamadı - ID: {model.ServiceId}");
                    return Json(new { success = false, message = $"Seçilen hizmet bulunamadı. (ID: {model.ServiceId})", debug = $"Database'de: {string.Join(", ", allServices.Select(s => $"{s.Id}:{s.Name}"))}" });
                }

                var allArtists = await _context.Artists.ToListAsync();
                _logger.LogInformation($"Database'de {allArtists.Count} sanatçı var: {string.Join(", ", allArtists.Select(a => $"{a.Id}:{a.FirstName}"))}" );

                var artist = await _context.Artists.FindAsync(model.ArtistId);
                if (artist == null)
                {
                    _logger.LogWarning($"Artist bulunamadı - ID: {model.ArtistId}");
                    return Json(new { success = false, message = $"Seçilen sanatçı bulunamadı. (ID: {model.ArtistId})", debug = $"Database'de: {string.Join(", ", allArtists.Select(a => $"{a.Id}:{a.FirstName}"))}" });
                }

                var appointment = new Appointment
                {
                    UserId = userId,
                    ArtistId = model.ArtistId,
                    ServiceId = model.ServiceId,
                    AppointmentDate = model.AppointmentDate,
                    TimeSlot = model.TimeSlot,
                    Description = model.Description,
                    GuestName = model.GuestName,
                    GuestEmail = model.GuestEmail,
                    GuestPhone = model.GuestPhone,
                    Price = service.Price,
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Randevu başarıyla alındı! Admin tarafından onaylanacaktır." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Appointment booking error");
                return Json(new { success = false, message = $"Hata: {ex.Message}", details = ex.InnerException?.Message });
            }
        }

        [HttpGet("debug")]
        public async Task<IActionResult> Debug()
        {
            var artists = await _context.Artists.ToListAsync();
            var services = await _context.Services.ToListAsync();

            var artistList = artists.Select(a => new { a.Id, a.FirstName, a.LastName, a.IsActive }).ToList();
            var serviceList = services.Select(s => new { s.Id, s.Name, s.Price, s.IsActive }).ToList();

            return Json(new
            {
                artists = artistList,
                services = serviceList,
                artistCount = artists.Count,
                serviceCount = services.Count
            });
        }
    }

    public class AppointmentViewModel
    {
        public int ArtistId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? TimeSlot { get; set; }
        public string? Description { get; set; }
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }
        public string? GuestPhone { get; set; }
    }
}
