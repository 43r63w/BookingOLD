
using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.ViewModels;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities"),
                Nights = 1,

                CheckInDate = DateOnly.FromDateTime(DateTime.Now)
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            Thread.Sleep(1000);

            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities").ToList();
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.BookingVilla.GetAll(u => u.Status == SD.StatusApproved || u.Status == SD.StatusCheckedIn).ToList();


            foreach (var villa in villaList)
            {
                var roomsAvialabel = SD.VillaRoomsAvailable_Count
               (villa.Id, villaNumberList, checkInDate, nights, bookedVillas);

                villa.IsAvalibel = roomsAvialabel > 0 ? true : false;
            }
            HomeVM homeVm = new()
            {
                VillaList = villaList,
                CheckInDate = checkInDate,
                Nights = nights

            };

            return PartialView("_VillasList", homeVm);

        }


        public IActionResult Privacy()
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
