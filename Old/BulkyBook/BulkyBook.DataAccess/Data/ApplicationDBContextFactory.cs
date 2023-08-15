using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            //JSON Path depends on where it is saved.
            string JsonFilePath = "C:\\Users\\kentz\\Documents\\GitHub\\BulkyBook\\BulkyBook\\BulkyBookWeb\\appsettings.json";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(JsonFilePath)
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("BulkyBook.DataAccess"));

            return new ApplicationDBContext(builder.Options);
        }
    }
}
