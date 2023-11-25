using Booking.Application.Interfaces;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Booking.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int id)
        {
            if (id == 0)
            {
                return View(new Villa());
            }

            var fromDb = _unitOfWork.Villa.GetValue(u => u.Id == id);
            return View(fromDb);
        }

        [HttpPost]
        public IActionResult Upsert(Villa villa)
        {
            if (ModelState.IsValid)
            {
                if (villa.Id == 0)
                {
                    _unitOfWork.Villa.Add(villa);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Villa.Update(villa);
                    _unitOfWork.Save();
                }
                if (villa.Image != null)
                {
                    if (villa.ImageUrl != null)
                    {
                        var imagePath = _webHostEnvironment.WebRootPath + "\\" + villa.ImageUrl.Trim('\\');

                        System.IO.File.Delete(imagePath);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string villaFolderPath = @"images\villa\" + villa.Name;
                    string finalPath = Path.Combine(wwwRootPath, villaFolderPath);

                    if (Directory.Exists(finalPath) == false)
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        villa.Image.CopyTo(fileStream);
                    };

                    villa.ImageUrl = villaFolderPath + "\\" + fileName;
                    _unitOfWork.Villa.Update(villa);
                    _unitOfWork.Save();

                    TempData["success"] = "Villa was update/created";
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
        #region APICALLS
        public IActionResult GetAll()
        {

            IEnumerable<Villa> villaLists = _unitOfWork.Villa.GetAll();

            return Json(new { data = villaLists });

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var fromDb = _unitOfWork.Villa.GetValue(u => u.Id == id);


            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath + @"\\"+ @"images\villa\" + fromDb.Name);
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, fromDb.ImageUrl);

            if (Directory.Exists(directoryPath))
            {
                System.IO.File.Delete(finalPath);

            }
            Directory.Delete(directoryPath);

            _unitOfWork.Villa.Remove(fromDb);
            _unitOfWork.Save();


            return Json(new { success = true, message = "Villa's number has been deleted" });
        }

        #endregion


    }
}
