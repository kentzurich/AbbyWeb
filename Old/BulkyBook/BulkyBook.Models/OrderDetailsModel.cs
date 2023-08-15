using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
	public class OrderDetailsModel
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int OrderId { get; set; }
		[ForeignKey("OrderId")]
		[ValidateNever]
		public OrderHeaderModel OrderHeader { get; set; }
		[Required]
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public ProductModel Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
