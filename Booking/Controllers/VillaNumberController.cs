using Booking.Application.Interfaces;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Diagnostics;
using Booking.Domain.ViewModels;
using System.Collections.Immutable;

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

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }).ToList(),

                VillaNumber = new VillaNumber()
            };

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM villaNumberVM)
        {
            var IsNumberAlreadyExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (IsNumberAlreadyExists)
            {
                TempData["warning"] = "This numbers already exists";
                villaNumberVM.villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }).ToList();

                return View(villaNumberVM);
            }
            _unitOfWork.VillaNumber.Add(villaNumberVM.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "The villa has been assigned a number";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int villa_Number)
        {

            IEnumerable<SelectListItem> villas = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            ViewBag.VillaList = villas;

            var villaFromDb = _unitOfWork.VillaNumber.GetValue(u => u.Villa_Number == villa_Number);

            return View(villaFromDb);
        }

        [HttpPost]
        public IActionResult Edit(VillaNumber villaNumber)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Villa was Update";
                return RedirectToAction(nameof(Index));
            }


            return View(villaNumber);
        }

        #region APICALLS
        public IActionResult GetAll()
        {
            IEnumerable<SelectListItem> villaLists = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            IEnumerable<VillaNumber> villaNumbers = _unitOfWork.VillaNumber.GetAll();

            return Json(new { data = villaNumbers });
        }

        [HttpDelete]
        public IActionResult Delete(int villa_Number)
        {
            var fromDb = _unitOfWork.VillaNumber.GetValue(u => u.Villa_Number == villa_Number);

            _unitOfWork.VillaNumber.Remove(fromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Villa's number has been deleted" });
        }

        #endregion
    }
}
