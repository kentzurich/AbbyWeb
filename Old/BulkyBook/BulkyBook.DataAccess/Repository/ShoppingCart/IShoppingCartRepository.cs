using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.ShoppingCart
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCartModel> 
    { 
        int IncrementCount(ShoppingCartModel shoppingCartModel, int count);
        int DecrementCount(ShoppingCartModel shoppingCartModel, int count);
    }
}
