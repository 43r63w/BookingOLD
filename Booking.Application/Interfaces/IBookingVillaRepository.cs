using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Interfaces
{
    public interface IBookingVillaRepository : IRepository<BookingVilla>
    {
        void Update(BookingVilla obj);

        void UpdateStatus(int bookingId, string bookingStatus,int villaNumber=0);
        void UpdatePaymentStatus(int bookingId, string sessionId, string paymentIntentId);
    }
}
