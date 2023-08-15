using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Orders
{
	public interface IOrderDetailsRepository : IGenericRepository<OrderDetailsModel>
	{
		void Update(OrderDetailsModel obj);
	}
}
