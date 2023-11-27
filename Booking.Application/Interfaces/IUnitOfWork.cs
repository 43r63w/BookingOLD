using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IVillaRepository Villa { get; }
        public IVillaNumberRepository VillaNumber { get; }
        public IAmenityRepository Amenity { get; }

        public IApplicationUserRepository User { get; }

        public IBookingVillaRepository BookingVilla { get; }


        void Save();
    }
}
