using BulkyBook.DataAccess.Repository.Category;
using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Orders
{
	public class OrderDetailsRepository : GenericRepository<OrderDetailsModel>, IOrderDetailsRepository
	{
		private ApplicationDBContext _db;
		public OrderDetailsRepository(ApplicationDBContext db) : base(db)
		{
			_db = db;
		}

		void IOrderDetailsRepository.Update(OrderDetailsModel obj)
		{
			_db.OrderDetails.Update(obj);
		}
	}
}
