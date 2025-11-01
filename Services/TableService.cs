using Microsoft.EntityFrameworkCore;
using Restaurant.Cli.Data;
using Restaurant.Cli.Models;

namespace Restaurant.Cli.Services;

public class TableService
{
    private readonly AppDbContext _db;

    public TableService(AppDbContext db) => _db = db;

    public List<Table> GetAll() =>
        _db.Tables.AsNoTracking().OrderBy(x => x.Number).ToList();

    public Table? GetByNumber(int number) =>
        _db.Tables.FirstOrDefault(x => x.Number == number);

    public void Create(int number, int seats, string status)
    {
        if (_db.Tables.Any(x => x.Number == number))
            throw new Exception($"Table #{number} already exists.");

        _db.Tables.Add(new Table { Number = number, Seats = seats, Status = status });
        _db.SaveChanges();
    }

    public void Update(int number, int? newSeats, string? newStatus)
    {
        var table = _db.Tables.FirstOrDefault(x => x.Number == number)
                    ?? throw new Exception($"Table #{number} not found.");

        if (newSeats is not null) table.Seats = newSeats.Value;
        if (!string.IsNullOrWhiteSpace(newStatus)) table.Status = newStatus;
        _db.SaveChanges();
    }

    public void Delete(int number)
    {
        var table = _db.Tables.FirstOrDefault(x => x.Number == number)
                    ?? throw new Exception($"Table #{number} not found.");

        var hasOrders = _db.Orders.Any(x => x.TableId == table.Id);
        if (hasOrders)
            throw new Exception("Cannot delete a table that has orders.");

        _db.Tables.Remove(table);
        _db.SaveChanges();
    }
}