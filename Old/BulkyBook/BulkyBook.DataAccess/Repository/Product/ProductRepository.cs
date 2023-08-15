using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;
using System.Security.Cryptography.X509Certificates;

namespace BulkyBook.DataAccess.Repository.Product
{
    public class ProductRepository : GenericRepository<ProductModel>, IProductRepository
    {
        private ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        void IProductRepository.Update(ProductModel obj)
        {
            var objFromDb = _db.Product.FirstOrDefault(x => x.Id == obj.Id);

            if(objFromDb is not null) 
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price = obj.Price;
                objFromDb.Price_50 = obj.Price_50;
                objFromDb.Price_100 = obj.Price_100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                objFromDb.CoverTypeId = obj.CoverTypeId;

                if(obj is not null)
                    objFromDb.ImageUrl = obj.ImageUrl;
            }
        }
    }
}
