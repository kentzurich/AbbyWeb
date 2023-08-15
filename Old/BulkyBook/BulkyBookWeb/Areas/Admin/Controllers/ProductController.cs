using BulkyBook.DataAccess.Repository.UnitOfWork;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.ProjectModel;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = StaticDetails.ROLE_ADMIN)]
	public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Edit Action Method
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                productModel = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
            };
            if (id is null || id.Equals(0))
            {
                //create
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productViewModel);
            }
            else
            {
                //update
                productViewModel.productModel = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                return View(productViewModel);
            }
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel obj, IFormFile? imgFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(imgFile is not null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"img\products");
                    var extension = Path.GetExtension(imgFile.FileName);

                    if(obj.productModel.ImageUrl is not null) // if image is exists in db
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.productModel.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        imgFile.CopyTo(fileStreams);
                    }
                       
                    obj.productModel.ImageUrl = @"\img\products\" + filename + extension;
                }

                if (obj.productModel.Id.Equals(0))
                {
                    _unitOfWork.Product.Add(obj.productModel);
                    TempData["success"] = "Product created succesfully.";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.productModel);
                    TempData["success"] = "Product updated succesfully.";
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
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }
        #endregion

        //POST Action Method
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductObj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (ProductObj is null)
                return Json(new { success = false, message = "Error while deleting." });

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, ProductObj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            _unitOfWork.Product.Remove(ProductObj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully." });
        }
    }
}