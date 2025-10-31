namespace Restaurant.Cli.Models;

public class Table
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Seats { get; set; }
    public string Status { get; set; } = "Available";

    public ICollection<Order>? Orders {get; set; }
}