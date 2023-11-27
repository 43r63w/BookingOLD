using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services
{
    public class Stripe
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
    }
}
