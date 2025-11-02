using Restaurant.Cli.Data;
using Restaurant.Cli.Models;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Cli.Services;

public class OrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public Order OpenOrder(int tablenumber)
    {
        var table = _db.Tables.FirstOrDefault(x => x.Number == tablenumber)
            ?? throw new Exception($"Table #{tablenumber} can not be found");

        if (_db.Orders.Any(x => x.TableId == table.Id && x.Status == "Open"))
        {
            throw new Exception($"Table #{tablenumber} already has an open order");
        }

        var order = new Order
        {
            TableId = table.Id,
            Status = "Open"
        };

        _db.Orders.Add(order);
        _db.SaveChanges();
        return order;
    }

    public void AddItem(int orderId, int MenuItemId, int quantity)
    {
        var order = _db.Orders.FirstOrDefault(x => x.Id == orderId)
            ?? throw new Exception($"Order not found");

        if (order.Status != "Open") throw new Exception("Order is closed");

        var menuItem = _db.MenuItems.FirstOrDefault(x => x.Id == MenuItemId)
            ?? throw new Exception("Menu Item not found");

        if (!menuItem.IsAvailable) throw new Exception("Menu Item is currently unavailable");

        var exist = _db.OrderItems.FirstOrDefault(x => x.OrderId == orderId && x.MenuItemId == MenuItemId);
        if (exist == null)
        {
            _db.OrderItems.Add(new OrderItem
            {
                OrderId = orderId,
                MenuItemId = MenuItemId,
                Quantity = quantity,
                UnitPrice = menuItem.Price
            });
        }
        else
        {
            exist.Quantity += quantity;
        }

        _db.SaveChanges();
    }

    public void RemoveItem(int orderId, int menuItemId)
    {
        var item = _db.OrderItems.FirstOrDefault(x => x.OrderId == orderId && x.MenuItemId == menuItemId)
            ?? throw new Exception("Item not found in order");

        _db.OrderItems.Remove(item);
        _db.SaveChanges();
    }

    public void CloseOrder(int orderId)
    {
        var order = _db.Orders.Include(x => x.Items).FirstOrDefault(x => x.Id == orderId)
            ?? throw new Exception($"Order not found");

        if (order.Status == "Closed") throw new Exception("Order is already closed");

        order.Status = "Closed";
        order.ClosedAt = DateTime.Now;
        _db.SaveChanges();
    }

    public List<Order> GetOpenOrders() =>
        _db.Orders
            .Include(x => x.Table)
            .Include(x => x.Items).ThenInclude(x => x.MenuItem)
            .Where(x => x.Status == "Open")
            .OrderBy(x => x.CreatedAt)
            .ToList();

    public decimal GetTotal(int orderId)
    {
        var order = _db.Orders.Include(x => x.Items).FirstOrDefault(x => x.Id == orderId)
            ?? throw new Exception("Order not found");
        return order.Items.Sum(x => x.Quantity * x.UnitPrice);
    }

}