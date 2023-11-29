using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ViewModels
{
    public class BookingVillaVM
    {
        public BookingVilla BookingVilla { get; set; }  
        public IEnumerable<Amenity> Amenities { get; set; }

    }
}
