namespace BulkyBook.Models.ViewModels
{
	public class OrderViewModel
	{
		public OrderHeaderModel OrderHeader { get; set; }
		public IEnumerable<OrderDetailsModel> OrderDetails { get; set; }
	}
}
