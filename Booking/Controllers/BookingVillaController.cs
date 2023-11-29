using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Booking.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Stripe.Checkout;
using System.Runtime.InteropServices;
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
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookingList = _unitOfWork.BookingVilla.GetAll(u=>u.Status==SD.StatusApproved|| u.Status==SD.StatusCheckedIn).ToList();

            bookingVilla.Price = villaDetail.Price * bookingVilla.Nights;

            bookingVilla.Status = SD.StatusPending;
            bookingVilla.BookingDate = DateTime.Now;


            int roomAvialabel = SD.VillaRoomsAvailable_Count(villaDetail.Id, villaNumberList, bookingVilla.CheckInDate, bookingVilla.Nights, bookingList);

            if (roomAvialabel == 0)
            {
                TempData["warning"] = "Rooms has been sold out";

                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    villaId = bookingVilla.Id,
                    checkInDate = bookingVilla.CheckInDate,
                    nights = bookingVilla.Nights,
                });


            }
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {

            BookingVillaVM bookingVillaVM = new()
            {
                BookingVilla = _unitOfWork.BookingVilla.GetValue(U => U.Id == id, includeProperties: "User,Villa"),
            };

            bookingVillaVM.Amenities = _unitOfWork.Amenity.GetAll(u => u.VillaId == bookingVillaVM.BookingVilla.Villa.Id);

            if (bookingVillaVM.BookingVilla.VillaNumber == 0 && bookingVillaVM.BookingVilla.Status == SD.StatusApproved)
            {
                var avialableVillaNumber = AssignAvialableVillaNumberByVilla(bookingVillaVM.BookingVilla.VillaId);


                bookingVillaVM.BookingVilla.VillaNumbers = _unitOfWork.VillaNumber.
                    GetAll(u => u.VillaId == bookingVillaVM.BookingVilla.VillaId && avialableVillaNumber.Any(x => x == u.Villa_Number)).ToList();
            }

            return View(bookingVillaVM);
        }


        public IActionResult CompleteBooking()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckIn(BookingVillaVM bookingVillaVM)
        {

            _unitOfWork.BookingVilla.UpdateStatus(bookingVillaVM.BookingVilla.Id, SD.StatusCheckedIn, bookingVillaVM.BookingVilla.VillaNumber);
            _unitOfWork.Save();


            TempData["success"] = "Booking updated";
            return RedirectToAction(nameof(Detail), new { id = bookingVillaVM.BookingVilla.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckoOut(BookingVillaVM bookingVillaVM)
        {

            _unitOfWork.BookingVilla.UpdateStatus(bookingVillaVM.BookingVilla.Id, SD.StatusCompleted, bookingVillaVM.BookingVilla.VillaNumber);
            _unitOfWork.Save();


            TempData["success"] = "Booking completed";
            return RedirectToAction(nameof(Detail), new { id = bookingVillaVM.BookingVilla.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking(BookingVillaVM bookingVillaVM)
        {

            _unitOfWork.BookingVilla.UpdateStatus(bookingVillaVM.BookingVilla.Id, SD.StatusCancelled);
            _unitOfWork.Save();

            var service = new SessionService();
            //Session session = service.Create();



            TempData["success"] = "Booking cancelled";
            return RedirectToAction(nameof(Detail), new { id = bookingVillaVM.BookingVilla.Id });
        }



        private List<int> AssignAvialableVillaNumberByVilla(int villadId)
        {
            List<int> availableVillaNumber = new();


            var villaNumbers = _unitOfWork.VillaNumber.GetAll(u => u.VillaId == villadId);

            var checkedVilla = _unitOfWork.BookingVilla.GetAll(u => u.VillaId == villadId && u.Status == SD.StatusCheckedIn).Select(u => u.VillaNumber);

            foreach (var villaNumber in villaNumbers)
            {
                if (!checkedVilla.Contains(villaNumber.Villa_Number))
                {
                    availableVillaNumber.Add(villaNumber.Villa_Number);
                }
            }

            return availableVillaNumber;
        }

        #region APICALLS
        public IActionResult GetAll(string status)
        {
            IEnumerable<BookingVilla> bookingVillas;

            if (User.IsInRole(SD.Role_Admin))
            {
                bookingVillas = _unitOfWork.BookingVilla.GetAll(includeProperties: "Villa,User").ToList();
            }
            else
            {
                var claim = (ClaimsIdentity)User.Identity;
                var userId = claim.FindFirst(ClaimTypes.NameIdentifier).Value;
                bookingVillas = _unitOfWork.BookingVilla.GetAll(u => u.UserId == userId, includeProperties: "Villa,User");

            }

            if (!string.IsNullOrEmpty(status))
            {
                bookingVillas = bookingVillas.Where(u => u.Status.ToLower().Equals(status.ToLower()));
            }

            return Json(new { data = bookingVillas });
        }
        #endregion




    }
}
