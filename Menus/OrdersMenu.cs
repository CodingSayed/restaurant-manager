using Restaurant.Cli.Services;

namespace Restaurant.Cli.Menus;

public class OrdersMenu
{
    private readonly OrderService _orderService;
    private readonly MenuItemService _menuItemService;

    public OrdersMenu(OrderService orderService, MenuItemService menuItemService)
    {
        _orderService = orderService;
        _menuItemService = menuItemService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelpers.PrintHeader("Orders");
            Console.WriteLine("1) List open orders");
            Console.WriteLine("2) Open new order");
            Console.WriteLine("3) Add item");
            Console.WriteLine("4) Remove item");
            Console.WriteLine("5) Close order");
            Console.WriteLine("0) Back");
            Console.Write("\nSelect a number: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            try
            {
                switch (choice)
                {
                    case "1": ListOpen(); break;
                    case "2": Open(); break;
                    case "3": AddItem(); break;
                    case "4": RemoveItem(); break;
                    case "5": Close(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            ConsoleHelpers.Pause();
        }

    }

    private void ListOpen()
    {
        var list = _orderService.GetOpenOrders();
        if (list.Count == 0)
        {
            Console.WriteLine("No open orders");
            return;
        }

        Console.WriteLine($"\n{"ID",-3}| {"Table",-5}| {"Items",-5}| {"Total (€)",8:F2}");
        Console.WriteLine("---------------------------------------------------------------");
        foreach (var x in list)
        {
            var total = x.Items.Sum(x => x.Quantity * x.UnitPrice);
            Console.WriteLine($"{x.Id,-3}| {x.Table?.Number,-5}| {x.Items.Count,-5}| {total,8:F2}");
        }
    }

    private void Open()
    {
        var table = ConsoleHelpers.PromptInt("Table number", min: 1);
        var order = _orderService.OpenOrder(table);
        Console.WriteLine($"Opened order #{order.Id} for table {table}.");
    }

    private void AddItem()
    {
        var orderId = ConsoleHelpers.PromptInt("Order ID", min: 1);
        Console.WriteLine("\nMenu:");

        foreach (var x in _menuItemService.GetAll())
            Console.WriteLine($"{x.Id}) {x.Name} - €{x.Price}");

        var itemId = ConsoleHelpers.PromptInt("\nMenu Item ID", min: 1);
        var quantity = ConsoleHelpers.PromptInt("Quantity", min: 1);

        _orderService.AddItem(orderId, itemId, quantity);
        Console.WriteLine("Item added");
    }

    private void RemoveItem()
    {
        var orderId = ConsoleHelpers.PromptInt("Order ID", min: 1);
        var menuItemId = ConsoleHelpers.PromptInt("Menu Item ID to remove", min: 1);
        _orderService.RemoveItem(orderId, menuItemId);
        Console.WriteLine("Item removed");
    }
    
    private void Close()
    {
        var orderId = ConsoleHelpers.PromptInt("Order ID", min: 1);
        var total = _orderService.GetTotal(orderId);

        if (!ConsoleHelpers.Confirm($"Close order #{orderId}? Total €{total:F2}")) return;

        _orderService.CloseOrder(orderId);
        Console.WriteLine("Order closed");
    }





}