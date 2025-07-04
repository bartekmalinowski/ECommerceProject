using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Dtos
{
    public class CreateOrderDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}