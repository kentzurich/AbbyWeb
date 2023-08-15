using BulkyBook.DataAccess.Repository.UnitOfWork;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryModel> categoryList = _unitOfWork.Category.GetAll();
            return View(categoryList);
        }

        //GET Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel obj)
        {
            CustomValidation(obj);

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created succesfully.";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        //Edit Action Method
        public IActionResult Edit(int? id)
        {
            if (id is null || id.Equals(0))
                return NotFound();

            //var categoryFromDB = _db.Categories.Find(id); // if you will find a primary key
            var categoryFromDBFirst = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            //var categoryFromDBSingle = _db.Categories.SingleOrDefault(x => x.Id == id);

            if (categoryFromDBFirst is null)
                return NotFound();

            return View(categoryFromDBFirst);
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel obj)
        {
            CustomValidation(obj);

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated succesfully.";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        //Delete Action Method
        public IActionResult Delete(int? id)
        {
            if (id is null || id.Equals(0))
                return NotFound();

            //var categoryFromDB = _db.Categories.Find(id);
            var categoryFromDBFirst = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            //var categoryFromDBSingle = _db.Categories.SingleOrDefault(x => x.Id == id);

            if (categoryFromDBFirst is null)
                return NotFound();

            return View(categoryFromDBFirst);
        }

        //POST Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoryObj = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (categoryObj is null)
                return NotFound();

            _unitOfWork.Category.Remove(categoryObj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted succesfully.";
            return RedirectToAction("Index");
        }

        // Server Side Validations
        private void CustomValidation(CategoryModel obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
                //ModelState.AddModelError("CustomError", "The display order cannot exactly match the Name.");
                ModelState.AddModelError("Name", "The display order cannot exactly match the Name.");
        }
    }
}
