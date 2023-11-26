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

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            return View();
        }


        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                villaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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

            amenityVM.villaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Edit(int id)
        {
            AmenityVM amenityVM = new()
            {
                villaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.GetValue(u => u.Id == id)
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


        #region APICALLS
        public IActionResult GetAll()
        {
            IEnumerable<SelectListItem> villas = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            IEnumerable<Amenity> amenitiesList = _unitOfWork.Amenity.GetAll().ToList();
            return Json(new { data = amenitiesList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var fromDb = _unitOfWork.Amenity.GetValue(u => u.Id == id);
            _unitOfWork.Amenity.Remove(fromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Amenity delete" });

        }

        #endregion






    }
}
