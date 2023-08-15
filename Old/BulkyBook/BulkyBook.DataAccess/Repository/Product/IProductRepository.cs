using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Product
{
    public interface IProductRepository : IGenericRepository<ProductModel>
    {
        void Update(ProductModel obj);
    }
}
