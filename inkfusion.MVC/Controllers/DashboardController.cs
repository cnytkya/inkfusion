using Microsoft.AspNetCore.Mvc;

namespace inkfusion.MVC.Controllers
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
