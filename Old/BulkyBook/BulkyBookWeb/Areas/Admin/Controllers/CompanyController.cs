using BulkyBook.DataAccess.Repository.UnitOfWork;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = StaticDetails.ROLE_ADMIN)]
	public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Edit Action Method
        public IActionResult Upsert(int? id)
        {
            CompanyModel obj = new();

            if (id is null || id.Equals(0))
                return View(obj);

            obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            return View(obj);
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CompanyModel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id.Equals(0))
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created succesfully.";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated succesfully.";
                }
                _unitOfWork.Save();
               
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }
        #endregion

        //POST Action Method
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyObj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

            if (CompanyObj is null)
                return Json(new { success = false, message = "Error while deleting." });

            _unitOfWork.Company.Remove(CompanyObj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company deleted successfully." });
        }
    }
}