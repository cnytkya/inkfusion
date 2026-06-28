using inkfusion.MVC.Models;
using inkfusion.MVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace inkfusion.MVC.Controllers
{
    public class SignupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SignupController> _logger;

        public SignupController(ApplicationDbContext context, ILogger<SignupController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("signup")]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public IActionResult Index(SignupModel model)
        {
            // Şifre eşleşme kontrolü
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Şifreler eşleşmiyor");
            }

            if (ModelState.IsValid)
            {
                // E-posta ve Kullanıcı Adı kontrolü
                bool emailExists = _context.Users.Any(u => u.Email.ToLower() == model.Email.ToLower());
                bool usernameExists = _context.Users.Any(u => u.Username.ToLower() == model.Username.ToLower());

                if (emailExists && usernameExists)
                {
                    ModelState.AddModelError("", "Bu e-posta adresi ve kullanıcı adı zaten kayıtlı");
                    return View(model);
                }

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı");
                    return View(model);
                }

                if (usernameExists)
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

                return RedirectToAction("Success");
            }
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
