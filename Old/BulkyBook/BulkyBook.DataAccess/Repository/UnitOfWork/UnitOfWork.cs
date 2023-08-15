using BulkyBook.DataAccess.Repository.ApplicationUser;
using BulkyBook.DataAccess.Repository.Category;
using BulkyBook.DataAccess.Repository.Company;
using BulkyBook.DataAccess.Repository.CoverType;
using BulkyBook.DataAccess.Repository.Orders;
using BulkyBook.DataAccess.Repository.Product;
using BulkyBook.DataAccess.Repository.ShoppingCart;

namespace BulkyBook.DataAccess.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _db;
        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;

            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }

        void IUnitOfWork.Save()
        {
            _db.SaveChanges();
        }
    }
}
