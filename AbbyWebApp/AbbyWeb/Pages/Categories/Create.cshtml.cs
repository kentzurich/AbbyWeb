using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDBContext _db;

        //[BindProperty] // for single model
        public CategoryModel category { get; set; }

        public CreateModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("category.Name", "The display order cannot exactly match the name.");
            }

            if(ModelState.IsValid) {
                await _db.Categories.AddAsync(category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category created succesfully.";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
