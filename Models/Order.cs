using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);
        public virtual List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
    }
}