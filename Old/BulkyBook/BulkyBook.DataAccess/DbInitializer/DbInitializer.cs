using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDBContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            //migrations if they are not  applied
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(StaticDetails.ROLE_ADMIN).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_ADMIN)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_EMPLOYEE)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_USER_INDIVIDUAL)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_USER_COMPANY)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUserModel
                {
                    UserName = "kathsss16@gmail.com",
                    Email = "kathsss16@gmail.com",
                    Name = "Kent Zurich",
                    PhoneNumber = "1234567890",
                    StreetAddress = "test address",
                    State = "test state",
                    PostalCode = "1423",
                    City = "test city"
                }, "Adm1n_1234").GetAwaiter().GetResult();

                ApplicationUserModel user = _db.ApplicationUser.FirstOrDefault(x => x.Email == "kathsss16@gmail.com");
                _userManager.AddToRoleAsync(user, StaticDetails.ROLE_ADMIN).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
