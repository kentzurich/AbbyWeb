using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;
using Newtonsoft.Json.Bson;

namespace BulkyBook.DataAccess.Repository.Orders
{
	public interface IOrderHeaderRepository : IGenericRepository<OrderHeaderModel>
	{
		void Update(OrderHeaderModel obj);
		void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
		void UpdateStripeSessionId(int id, string sessionId);
		void UpdateStripePaymentId(int id, string paymentIntentId);
	}
}
