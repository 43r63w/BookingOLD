using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
