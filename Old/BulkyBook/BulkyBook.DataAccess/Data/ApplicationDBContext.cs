using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<CategoryModel> Categories { get; set; } // table
        public DbSet<CoverTypeModel> CoverType { get; set; } // table
        public DbSet<ProductModel> Product { get; set; } // table
        public DbSet<ApplicationUserModel> ApplicationUser { get; set; } // table
        public DbSet<CompanyModel> Company { get; set; } // table
        public DbSet<ShoppingCartModel> ShoppingCart { get; set; } // table
        public DbSet<OrderHeaderModel> OrderHeader { get; set; } // table
        public DbSet<OrderDetailsModel> OrderDetails { get; set; } // table
    }
}
