using Booking.Application.Interfaces;
using Booking.Infrustucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrustucture.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Villa = new VillaRepository(_context);
            VillaNumber = new VillaNumberRepository(_context);
            Amenity = new AmenityRepository(_context);
            User = new ApplicationUserRepository(_context);
            BookingVilla = new BookingVillaRepository(_context);    
        }

        public IVillaRepository Villa { get; private set; }

        public IVillaNumberRepository VillaNumber { get; private set; }

        public IAmenityRepository Amenity { get; private set; }


        public IBookingVillaRepository BookingVilla { get; private set; }

        public IApplicationUserRepository User { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
