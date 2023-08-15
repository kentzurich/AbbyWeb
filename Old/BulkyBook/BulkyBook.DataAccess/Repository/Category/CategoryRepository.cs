using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Category
{
    public class CategoryRepository : GenericRepository<CategoryModel>, ICategoryRepository
    {
        private ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        void ICategoryRepository.Update(CategoryModel obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
