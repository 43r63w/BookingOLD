using Booking.Application.Interfaces;
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
    }
}
