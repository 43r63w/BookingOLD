using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;

namespace Booking.Controllers
{
    [Authorize]
    public class BookingVillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingVillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult FinalizeBooking(int villaId, int nights, DateOnly checkInDate)
        {
            var claim = (ClaimsIdentity)User.Identity;
            var userId = claim.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userFromDb = _unitOfWork.User.GetValue(u => u.Id == userId);

            BookingVilla booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.GetValue(u => u.Id == villaId, includeProperties: "VillaAmenities"),
                Nights = nights,
                CheckInDate = checkInDate,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = userFromDb.PhoneNumber,
                Name = userFromDb.Name,
                Email = userFromDb.Email,

            };

            booking.Price = booking.Villa.Price * nights;

            return View(booking);
        }

        [HttpPost]
        public IActionResult FinalizeBooking(BookingVilla bookingVilla)
        {
            var villaDetail = _unitOfWork.Villa.GetValue(u => u.Id == bookingVilla.VillaId);

            bookingVilla.Price = villaDetail.Price * bookingVilla.Nights;

            bookingVilla.Status = SD.StatusPending;
            bookingVilla.BookingDate = DateTime.Now;

            _unitOfWork.BookingVilla.Add(bookingVilla);
            _unitOfWork.Save();

            return RedirectToAction(nameof(BookingConformation), new { bookingId = bookingVilla.Id });
        }

        public IActionResult BookingConformation(int bookingId)
        {
            return View(bookingId);
        }
    }
}
