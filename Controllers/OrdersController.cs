using ECommerceAPI.Data;
using ECommerceAPI.Dtos;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ApiDbContext _context;

    public OrdersController(ApiDbContext context)
    {
        _context = context;
    }

    // GET: api/Orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                CustomerName = o.CustomerName,
                TotalAmount = o.TotalAmount,
                Items = o.Items.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product!.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    // GET: api/Orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var orderDto = await _context.Orders
            .Where(o => o.Id == id)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                CustomerName = o.CustomerName,
                TotalAmount = o.TotalAmount,
                Items = o.Items.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product!.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (orderDto == null)
        {
            return NotFound();
        }

        return Ok(orderDto);
    }

    // POST: api/Orders
    // ZMIANA: Implementacja z walidacją i DTO
    [HttpPost]
    public async Task<ActionResult<OrderDto>> PostOrder(CreateOrderDto createOrderDto)
    {
        var newOrder = new Order
        {
            CustomerName = createOrderDto.CustomerName,
            OrderDate = DateTime.UtcNow
        };

        foreach (var itemDto in createOrderDto.Items)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId);
            if (product == null)
            {
                return BadRequest($"Produkt o ID {itemDto.ProductId} nie istnieje.");
            }

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                // WAŻNE: Cena jest pobierana z serwera, a nie od klienta!
                Price = product.Price
            };
            newOrder.Items.Add(orderItem);
        }

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        // Zwracamy pełne DTO nowo utworzonego zamówienia
        var resultDto = await GetOrder(newOrder.Id);
        return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, resultDto.Value);
    }

    // PUT: api/Orders/5
    // NOWOŚĆ: Implementacja modyfikacji
[HttpPut("{id}")]
public async Task<IActionResult> PutOrder(int id, UpdateOrderDto updateOrderDto)
{
    // 1. Znajdź istniejące zamówienie wraz z jego pozycjami
    var orderInDb = await _context.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == id);

    if (orderInDb == null)
    {
        return NotFound();
    }

    // 2. Zaktualizuj proste właściwości (np. nazwa klienta)
    orderInDb.CustomerName = updateOrderDto.CustomerName;

    // 3. Zastosuj logikę "Wipe and Replace" (Usuń i Zastąp) dla pozycji zamówienia
    // To najprostsza i najbezpieczniejsza strategia dla pełnej aktualizacji kolekcji.

    // Usuń stare pozycje
    _context.OrderItems.RemoveRange(orderInDb.Items);

    // Dodaj nowe pozycje na podstawie danych z DTO
    foreach (var itemDto in updateOrderDto.Items)
    {
        var product = await _context.Products.FindAsync(itemDto.ProductId);
        if (product == null)
        {
            // Jeśli jakiś produkt nie istnieje, odrzuć całą transakcję
            return BadRequest($"Produkt o ID {itemDto.ProductId} nie istnieje.");
        }

        var newOrderItem = new OrderItem
        {
            ProductId = product.Id,
            Quantity = itemDto.Quantity,
            Price = product.Price, // Zawsze pobieraj cenę z serwera!
            Product = product
        };
        orderInDb.Items.Add(newOrderItem);
    }
    
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Orders.Any(e => e.Id == id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }

    return NoContent(); // Sukces
}

    // DELETE: api/Orders/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        
        _context.OrderItems.RemoveRange(order.Items); // Najpierw usuń pozycje
        _context.Orders.Remove(order); // Potem zamówienie
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
}