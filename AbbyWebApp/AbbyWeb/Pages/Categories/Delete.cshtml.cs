using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDBContext _db;

        //[BindProperty] // for single model
        public CategoryModel category { get; set; }

        public DeleteModel(ApplicationDBContext db)
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
            var categoryFromDb = _db.Categories.Find(category.Id);

            if (categoryFromDb != null)
            {
                _db.Categories.Remove(categoryFromDb);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category deleted succesfully.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
