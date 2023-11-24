using Booking.Application.Interfaces;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }


        #region APICALLS
        public IActionResult GetAll()
        {
            IEnumerable<VillaNumber> villaNumbers = _unitOfWork;
            
        }

        #endregion
    }
}
