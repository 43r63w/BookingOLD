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
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _context; 

        public VillaNumberRepository(ApplicationDbContext context) : base(context)
        { 
            _context = context;
        }

        public void Update(VillaNumber obj)
        {
            _context.VillaNumbers.Update(obj);
        }
    }
}
