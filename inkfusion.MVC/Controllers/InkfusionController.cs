using Microsoft.AspNetCore.Mvc;

namespace inkfusion.MVC.Controllers
{
    public class InkfusionController : Controller
    {
        public IActionResult gallery()
        {
            return View();
        }
    }
}
