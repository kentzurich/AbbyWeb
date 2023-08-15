using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDBContext _db;

        //[BindProperty] // for single model
        public CategoryModel category { get; set; }

        public EditModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            category = _db.Categories.Find(id);
            //category = _db.Categories.FirstOrDefault(x => x.Id == id);
            //category = _db.Categories.SingleOrDefault(x => x.Id == id);
            //category = _db.Categories.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IActionResult> OnPost()
        {
            if(category.Name == category.DisplayOrder.ToString())
                ModelState.AddModelError("category.Name", "The display order cannot exactly match the name.");

            if(ModelState.IsValid) 
            {
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category updated succesfully.";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
