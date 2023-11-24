using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrustucture.Data.DbInitializers
{
 
    public class DbInitialize : IDbinitialize
    {

        private readonly ApplicationDbContext _context;

        public DbInitialize(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            try
            {

                _context.Database.EnsureCreated();

            }
            catch (Exception ex)
            {

            }

        }
    }
}
