using BulkyBook.DataAccess.Repository.UnitOfWork;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = StaticDetails.ROLE_ADMIN)]
	public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverTypeModel> coverTypeList = _unitOfWork.CoverType.GetAll();
            return View(coverTypeList);
        }

        //GET Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverTypeModel obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type created succesfully.";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        //Edit Action Method
        public IActionResult Edit(int? id)
        {
            if (id is null || id.Equals(0))
                return NotFound();

            var coverTypeFromDBFirst = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (coverTypeFromDBFirst is null)
                return NotFound();

            return View(coverTypeFromDBFirst);
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverTypeModel obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type updated succesfully.";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        //Delete Action Method
        public IActionResult Delete(int? id)
        {
            if (id is null || id.Equals(0))
                return NotFound();

            var coverTypeFromDBFirst = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (coverTypeFromDBFirst is null)
                return NotFound();

            return View(coverTypeFromDBFirst);
        }

        //POST Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var coverTypeObj = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (coverTypeObj is null)
                return NotFound();

            _unitOfWork.CoverType.Remove(coverTypeObj);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type deleted succesfully.";
            return RedirectToAction("Index");
        }
    }
}
