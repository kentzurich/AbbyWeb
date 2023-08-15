using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbbyWeb.Data;
using AbbyWeb.Model;

namespace AbbyWeb.Pages.CategoryTemp
{
    public class DeleteModel : PageModel
    {
        private readonly AbbyWeb.Data.ApplicationDBContext _context;

        public DeleteModel(AbbyWeb.Data.ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CategoryModel CategoryModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var categorymodel = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (categorymodel == null)
            {
                return NotFound();
            }
            else 
            {
                CategoryModel = categorymodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var categorymodel = await _context.Categories.FindAsync(id);

            if (categorymodel != null)
            {
                CategoryModel = categorymodel;
                _context.Categories.Remove(CategoryModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
