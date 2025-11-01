using Restaurant.Cli.Services;

namespace Restaurant.Cli.Menus;

public class TablesMenu
{
    private readonly TableService _tableService;

    public TablesMenu(TableService tableService)
    {
        _tableService = tableService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            ConsoleHelpers.PrintHeader("Tables");
            Console.WriteLine("1) List");
            Console.WriteLine("2) Create");
            Console.WriteLine("3) Edit");
            Console.WriteLine("4) Delete");
            Console.WriteLine("0) Back");
            Console.Write("\nSelect a number: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            try
            {
                switch (choice)
                {
                    case "1": List(); break;
                    case "2": Create(); break;
                    case "3": Edit(); break;
                    case "4": Delete(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            ConsoleHelpers.Pause();
        }
    }

    private void List()
    {
        var items = _tableService.GetAll();
        if (items.Count == 0) { Console.WriteLine("No tables found."); return; }

        Console.WriteLine("\n#  | Seats | Status");
        Console.WriteLine("--------------------");
        foreach (var t in items)
            Console.WriteLine($"{t.Number,-3}| {t.Seats,-5}| {t.Status}");
    }

    private void Create()
    {
        var number = ConsoleHelpers.PromptInt("Table number", min: 1);
        var seats = ConsoleHelpers.PromptInt("Seats", min: 1);
        var status = ConsoleHelpers.Prompt("Status [Available/Occupied/Reserved]", "Available");
        _tableService.Create(number, seats, status);
        Console.WriteLine("Table created.");
    }

    private void Edit()
    {
        var number = ConsoleHelpers.PromptInt("Table number to edit", min: 1);
        var seatsStr = ConsoleHelpers.Prompt("New seats (empty = keep)");
        int? newSeats = string.IsNullOrWhiteSpace(seatsStr) ? null : int.Parse(seatsStr);
        var statusStr = ConsoleHelpers.Prompt("New status (empty = keep)");
        _tableService.Update(number, newSeats, string.IsNullOrWhiteSpace(statusStr) ? null : statusStr);
        Console.WriteLine("Table updated.");
    }

    private void Delete()
    {
        var number = ConsoleHelpers.PromptInt("Table number to delete", min: 1);
        if (!ConsoleHelpers.Confirm($"Delete table #{number}?")) return;
        _tableService.Delete(number);
        Console.WriteLine("Table deleted.");
    }
}