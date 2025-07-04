// Dtos/UpdateOrderDto.cs
using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Dtos
{
    // ZMIANA: Ten DTO zawiera teraz pełne informacje do aktualizacji zamówienia
    public class UpdateOrderDto
    {
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}