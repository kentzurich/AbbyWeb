using BulkyBook.DataAccess.Repository.Category;
using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.ShoppingCart
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCartModel>, IShoppingCartRepository
    {
        private ApplicationDBContext _db;
        public ShoppingCartRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        public int DecrementCount(ShoppingCartModel shoppingCartModel, int count)
        {
            shoppingCartModel.Count -= count;
            return shoppingCartModel.Count;
        }

        public int IncrementCount(ShoppingCartModel shoppingCartModel, int count)
        {
            shoppingCartModel.Count += count;
            return shoppingCartModel.Count;
        }
    }
}
