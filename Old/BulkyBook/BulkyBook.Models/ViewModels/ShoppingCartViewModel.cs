namespace BulkyBook.Models.ViewModels
{
	public class ShoppingCartViewModel
	{
		public IEnumerable<ShoppingCartModel> ListCart { get; set; }
        public OrderHeaderModel OrderHeader { get; set; }
    }
}
