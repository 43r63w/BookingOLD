using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Booking.Infrustucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrustucture.Repository
{
    public class BookingVillaRepository : Repository<BookingVilla>, IBookingVillaRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingVillaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(BookingVilla obj)
        {
            _context.Bookings.Update(obj);
        }
        public void UpdateStatus(int bookingId, string bookingStatus)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(u => u.Id == bookingId);

            if (bookingFromDb != null)
            {
                bookingFromDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }
                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckOutDate = DateTime.Now;
                }

            }
        }

        public void UpdatePaymentStatus(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(u => u.Id == bookingId);
            if (bookingFromDb != null)
            {
                if (sessionId != null)
                {
                    bookingFromDb.StripeSessionId = sessionId;

                }
                if (paymentIntentId != null)
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromDb.PaymentDate = DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful = true;            
                }
            }
        }

    }
}
