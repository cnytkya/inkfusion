using inkfusion.MVC.Models;
using inkfusion.MVC.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace inkfusion.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ApplicationDbContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("login")]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u =>
                    (u.Email == model.Email || u.Username == model.Email) && u.IsActive);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim("Username", user.Username),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(model.RememberMe ? 30 : 1)
                    };

                    await HttpContext.SignInAsync(
                        "CookieAuth",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    user.LastLoginAt = DateTime.UtcNow;
                    _context.SaveChanges();

                    // Redirect based on role
                    if (user.Role == UserRole.Admin)
                    {
                        return RedirectToAction("Index", "AdminDashboard");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Geçersiz e-posta/kullanıcı adı veya şifre");
            }

            return View(model);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}
