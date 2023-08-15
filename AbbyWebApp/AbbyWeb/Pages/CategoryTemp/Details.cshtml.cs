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
    public class DetailsModel : PageModel
    {
        private readonly AbbyWeb.Data.ApplicationDBContext _context;

        public DetailsModel(AbbyWeb.Data.ApplicationDBContext context)
        {
            _context = context;
        }

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
    }
}
