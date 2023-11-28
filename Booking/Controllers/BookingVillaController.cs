using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Stripe.Checkout;
using System.Security.Claims;

namespace Booking.Controllers
{
    [Authorize]
    public class BookingVillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingVillaController> _logger;

        public BookingVillaController(IUnitOfWork unitOfWork,
            ILogger<BookingVillaController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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


            var domain = "https://localhost:7040/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"bookingvilla/BookingConformation?bookingId={bookingVilla.Id}",
                CancelUrl = domain + $"bookingvilla/FinalizeBooking?villaId={villaDetail.Id}&nights={bookingVilla.Nights}&checkInDate={bookingVilla.CheckInDate}",
            };



            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(bookingVilla.Price * 100),
                    Currency = "usd",

                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villaDetail.Name,

                    }
                },

                Quantity = 1
            });

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.BookingVilla.UpdatePaymentStatus(bookingVilla.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        public IActionResult BookingConformation(int bookingId)
        {
            try
            {
                var bookingFromDb = _unitOfWork.BookingVilla.GetValue(u => u.Id == bookingId, includeProperties: "User,Villa");

                if (bookingFromDb.Status == SD.StatusPending)
                {
                    var service = new SessionService();
                    Session session = service.Get(bookingFromDb.StripeSessionId);

                    if (session.PaymentStatus == "paid")
                    {
                        _unitOfWork.BookingVilla.UpdateStatus(bookingFromDb.Id, SD.StatusApproved);
                        _unitOfWork.BookingVilla.UpdatePaymentStatus(bookingId, session.Id, session.PaymentIntentId);
                        _unitOfWork.Save();
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}");
                _logger.LogInformation($"{ex.StackTrace}");
            }


            return View(bookingId);
        }
    }
}
