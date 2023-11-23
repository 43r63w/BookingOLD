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
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _context; 

        public VillaRepository(ApplicationDbContext context) : base(context)
        { 
            _context = context;
        }

        public void Update(Villa obj)
        {
            _context.Villas.Update(obj);
        }
    }
}
