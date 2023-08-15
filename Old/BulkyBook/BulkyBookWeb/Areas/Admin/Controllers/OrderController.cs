using BulkyBook.DataAccess.Repository.UnitOfWork;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public OrderViewModel OrderVM { get; set; }
		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details(int orderId)
		{
			OrderVM = new()
			{
				OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderId, includeProperties: "ApplicationUser"),
				OrderDetails = _unitOfWork.OrderDetails.GetAll(x => x.OrderId == orderId, includeProperties: "Product")
			};
			return View(OrderVM);
		}
		//[ActionName("Details")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Details_PayNow()
		{
			OrderVM.OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
			OrderVM.OrderDetails = _unitOfWork.OrderDetails.GetAll(x => x.OrderId == OrderVM.OrderHeader.Id, includeProperties: "Product");

            // Stripe Settings
            //var domainUrl = "https://localhost:44374/";
            var domainUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string>
				{
					"card",
				},
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
				SuccessUrl = $"{domainUrl}Admin/Order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
				CancelUrl = $"{domainUrl}Admin/Order/Details?orderId={OrderVM.OrderHeader.Id}",
			};

			foreach (var item in OrderVM.OrderDetails)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100),
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Title,
						},
					},
					Quantity = item.Count,
				};
				options.LineItems.Add(sessionLineItem);
			}

			var service = new SessionService();
			Session session = service.Create(options);
			_unitOfWork.OrderHeader.UpdateStripeSessionId(OrderVM.OrderHeader.Id, session.Id);
			_unitOfWork.Save();
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}

		public IActionResult PaymentConfirmation(int orderHeaderId)
		{
			OrderHeaderModel orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderHeaderId);
			if (orderHeader.PaymentStatus.Equals(StaticDetails.PAYMENTSTATUS_DELAYED) ||
				(orderHeader.PaymentStatus.Equals(StaticDetails.ORDERSTATUS_PENDING) &&
				 orderHeader.OrderStatus.Equals(StaticDetails.ORDERSTATUS_PENDING)))
			{
				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);
				//check stripe status
				if (session.PaymentStatus.ToLower().Equals("paid"))
				{
					_unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeader.Id, session.PaymentIntentId);
					_unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, StaticDetails.ORDERSTATUS_APPROVED, StaticDetails.PAYMENTSTATUS_APPROVED);
					_unitOfWork.Save();
				}
			}
			return View(orderHeaderId);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetail()
		{
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == OrderVM.OrderHeader.Id, isTracked:false);
			orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
			orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
			orderHeaderFromDb.City = OrderVM.OrderHeader.City;
			orderHeaderFromDb.State = OrderVM.OrderHeader.State;
			orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if(OrderVM.OrderHeader.Carrier is not null)
				orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;

			if (OrderVM.OrderHeader.TrackingNumber is not null)
				orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order details updated successfully.";
			return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.ROLE_ADMIN + "," + StaticDetails.ROLE_EMPLOYEE)]
		[ValidateAntiForgeryToken]
		public IActionResult StartProcessing()
		{
			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, StaticDetails.ORDERSTATUS_INPROCESS);
			_unitOfWork.Save();
			TempData["Success"] = "Order start processing.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.ROLE_ADMIN + "," + StaticDetails.ROLE_EMPLOYEE)]
		[ValidateAntiForgeryToken]
		public IActionResult ShipOrder()
		{
			var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == OrderVM.OrderHeader.Id, isTracked: false);
			orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			orderHeaderFromDb.OrderStatus = StaticDetails.ORDERSTATUS_SHIPPED;
			orderHeaderFromDb.ShippingDate = DateTime.Now;

			if(orderHeaderFromDb.OrderStatus.Equals(StaticDetails.PAYMENTSTATUS_DELAYED))
				orderHeaderFromDb.PaymentDueDate = DateTime.Now.AddDays(30);

			_unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();
			TempData["Success"] = "Order shipped successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.ROLE_ADMIN + "," + StaticDetails.ROLE_EMPLOYEE)]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == OrderVM.OrderHeader.Id, isTracked: false);
			if(orderHeaderFromDb.PaymentStatus.Equals(StaticDetails.PAYMENTSTATUS_APPROVED))
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderHeaderFromDb.PaymentIntentId
				};

				var service = new RefundService();
				Refund refund = service.Create(options);
				_unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, StaticDetails.ORDERSTATUS_CANCELLED, StaticDetails.ORDERSTATUS_REFUNDED);
			}
			else
				_unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, StaticDetails.ORDERSTATUS_CANCELLED, StaticDetails.ORDERSTATUS_CANCELLED);

			_unitOfWork.Save();
			TempData["Success"] = "Order cancelled successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeaderModel> orderHeader;

			if(User.IsInRole(StaticDetails.ROLE_ADMIN) || User.IsInRole(StaticDetails.ROLE_EMPLOYEE))
                orderHeader = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            else
			{
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeader = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == claim.Value,includeProperties: "ApplicationUser");
            }

			switch(status)
			{
                case "paymentpending":
					orderHeader = orderHeader.Where(x => x.PaymentStatus == StaticDetails.PAYMENTSTATUS_PENDING);
                    break;
                case "inprocess":
                    orderHeader = orderHeader.Where(x => x.OrderStatus == StaticDetails.ORDERSTATUS_INPROCESS);
                    break;
                case "completed":
                    orderHeader = orderHeader.Where(x => x.OrderStatus == StaticDetails.ORDERSTATUS_SHIPPED);
                    break;
                case "approved":
                    orderHeader = orderHeader.Where(x => x.OrderStatus == StaticDetails.ORDERSTATUS_APPROVED);
                    break;
                default:
                    break;
            }

			return Json(new { data = orderHeader });
		}
		#endregion
	}
}
