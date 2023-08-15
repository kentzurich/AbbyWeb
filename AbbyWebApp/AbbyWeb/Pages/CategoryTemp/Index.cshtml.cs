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
    public class IndexModel : PageModel
    {
        private readonly AbbyWeb.Data.ApplicationDBContext _context;

        public IndexModel(AbbyWeb.Data.ApplicationDBContext context)
        {
            _context = context;
        }

        public IList<CategoryModel> CategoryModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Categories != null)
            {
                CategoryModel = await _context.Categories.ToListAsync();
            }
        }
    }
}
