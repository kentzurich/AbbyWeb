using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Category
{
    public interface ICategoryRepository : IGenericRepository<CategoryModel>
    {
        void Update(CategoryModel obj);
    }
}
