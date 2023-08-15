using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Orders
{
	public class OrderHeaderRepository : GenericRepository<OrderHeaderModel>, IOrderHeaderRepository
	{
		private ApplicationDBContext _db;
		public OrderHeaderRepository(ApplicationDBContext db) : base(db)
		{
			_db = db;
		}

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _db.OrderHeader.FirstOrDefault(x => x.Id == id);
			if(orderFromDb is not null)
			{
				orderFromDb.OrderStatus = orderStatus;
				if(paymentStatus is not null)
					orderFromDb.PaymentStatus = paymentStatus;
			}
		}
		public void UpdateStripeSessionId(int id, string sessionId)
		{
			var orderFromDb = _db.OrderHeader.FirstOrDefault(x => x.Id == id);
			orderFromDb.SessionId = sessionId;
		}

		public void UpdateStripePaymentId(int id, string paymentIntentId)
		{
			var orderFromDb = _db.OrderHeader.FirstOrDefault(x => x.Id == id);
			orderFromDb.PaymentDate = DateTime.Now;
			orderFromDb.PaymentIntentId = paymentIntentId;
		}

		void IOrderHeaderRepository.Update(OrderHeaderModel obj)
		{
			_db.OrderHeader.Update(obj);
		}
	}
}
