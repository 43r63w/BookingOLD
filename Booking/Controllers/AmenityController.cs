using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Booking.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace Booking.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AmenityController> _logger;    

        public AmenityController(IUnitOfWork unitOfWork, 
            ILogger<AmenityController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Amenity> amenitiesList = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");

            return View(amenitiesList);
        }


        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),


                Amenity = new Amenity()
            };

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity implement";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Edit(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.GetValue(u => u.Id == amenityId)
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Edit(AmenityVM amenityVM)
        {
            var fromDb = _unitOfWork.Amenity.GetValue(u => u.Id == amenityVM.Amenity.Id);

            fromDb.Id = amenityVM.Amenity.Id;
            fromDb.Name = amenityVM.Amenity.Name;
            fromDb.Description = amenityVM.Amenity.Description;

            _unitOfWork.Amenity.Update(fromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int amenityId)
        {
            var fromDb = _unitOfWork.Amenity.GetValue(u => u.Id == amenityId);
            _unitOfWork.Amenity.Remove(fromDb);
            _unitOfWork.Save();

            TempData["success"] = "Amenity deleted";
            return RedirectToAction(nameof(Index));
        }



    }
}
