namespace Restaurant.Cli.Models;

public class Order
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public Table? Table { get; set; }

    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}