using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Dtos
{
    public class CreateOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}