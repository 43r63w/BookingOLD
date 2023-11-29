using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ViewModels
{
    public class RadialBarChartVM
    {

        public decimal TotalCount { get; set; }
        public decimal CountInCurrentMonth {  get; set; }   
        public bool HasRatioIncreased { get; set; }
        public int[] Series { get; set; }


    }
}
