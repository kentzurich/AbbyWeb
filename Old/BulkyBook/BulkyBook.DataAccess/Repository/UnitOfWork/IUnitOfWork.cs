using BulkyBook.DataAccess.Repository.ApplicationUser;
using BulkyBook.DataAccess.Repository.Category;
using BulkyBook.DataAccess.Repository.Company;
using BulkyBook.DataAccess.Repository.CoverType;
using BulkyBook.DataAccess.Repository.Orders;
using BulkyBook.DataAccess.Repository.Product;
using BulkyBook.DataAccess.Repository.ShoppingCart;

namespace BulkyBook.DataAccess.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeader { get; }
        void Save();
    }
}
