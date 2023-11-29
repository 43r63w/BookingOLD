using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";


        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string All = "All";


        public static int VillaRoomsAvailable_Count(int villadId, List<VillaNumber> villaNumbersList,
            DateOnly checkInDate, int nights, List<BookingVilla> bookingVillas)

        {
            List<int> bookingInDate = new();

            int finalAvialableRoomsForAllNights = int.MaxValue;

            var roomsVilla = villaNumbersList.Where(u => u.VillaId == villadId).Count();


            for (int i = 0; i < nights; i++)
            {
                var villasBooked = bookingVillas.Where(u => u.CheckInDate <= checkInDate.AddDays(i) &&
                u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villadId);

                foreach (var booking in villasBooked)
                {
                    if (!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }

                var totalAvialabelRooms = roomsVilla - bookingInDate.Count;
                if (totalAvialabelRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if (finalAvialableRoomsForAllNights > totalAvialabelRooms)
                    {
                        finalAvialableRoomsForAllNights = totalAvialabelRooms;
                    }
                }
            }

            return finalAvialableRoomsForAllNights;

        }


    }

}
