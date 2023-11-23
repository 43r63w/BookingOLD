using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class VillaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
