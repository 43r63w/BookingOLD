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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context; 

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        { 
            _context = context;
        }

        public void Update(ApplicationUser obj)
        {
            _context.ApplicationUsers.Update(obj);
        }
    }
}
