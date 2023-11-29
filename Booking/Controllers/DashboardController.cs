using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Booking.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;

        readonly DateTime previusMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);


        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.BookingVilla.GetAll(u => u.Status != SD.StatusPending || u.Status == SD.StatusCancelled);

            var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= currentMonthStartDate);


            RadialBarChartVM radialBarChartVM = new();



            int increaseDecreaseRatio = 100;


            if (countByPreviousMonth != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth * 100);
            }

            radialBarChartVM.TotalCount = totalBookings.Count();
            radialBarChartVM.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVM.HasRatioIncreased = currentMonthStartDate > previusMonthStartDate;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return Json(radialBarChartVM);

        }
    }
}
