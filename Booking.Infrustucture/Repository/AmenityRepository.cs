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
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext _context; 

        public AmenityRepository(ApplicationDbContext context) : base(context)
        { 
            _context = context;
        }

        public void Update(Amenity obj)
        {
            _context.Amenities.Update(obj);
        }
    }
}
