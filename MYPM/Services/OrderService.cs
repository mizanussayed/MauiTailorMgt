using Microsoft.EntityFrameworkCore;
using MYPM.Common;
using MYPM.Data.Configurations;
using MYPM.Models;

namespace MYPM.Services;

public class OrderService(IDbContextFactory<AppDbContext> dbFactory) : IOrderService
{
    private async Task<int> GetNextId(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var utcNow = DateTime.UtcNow;

        var startOfWeek = utcNow.Date.AddDays(-(int)utcNow.DayOfWeek -1);
        var endOfWeek = startOfWeek.AddDays(7); // exclusive

        var weeklyOrderCount = await db.Orders
            .CountAsync(o => o.OrderDate >= startOfWeek && o.OrderDate < endOfWeek, ct);

        return weeklyOrderCount + 1;
    }


    private static List<NewOrderModel> Normalize(List<NewOrderModel> orders)
        => [.. orders.OrderByDescending(o => o.Id)];

    public async Task<List<NewOrderModel>> GetAllOrders()
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var orders = await db.Orders
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
            var normalized = Normalize(orders);
            return normalized;
        }
        catch{ return []; }
    }

    public async Task<List<NewOrderModel>> GetCustomerOrders(string mobileNumber)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var orders = await db.Orders
                .Where(o => o.MobileNumber == mobileNumber)
                .OrderByDescending(o => o.Id)
                .AsNoTracking()
                .ToListAsync().ConfigureAwait(false);
            return orders;
        }
        catch { return []; }
    }

    public async Task<NewOrderModel> GetOrder(int id)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync().ConfigureAwait(false);
            var order = await db.Orders
                .Include(o => o.PanjabiOrders)
                .Include(o => o.ArabianOrders)
                .Include(o => o.SelowerOrders)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id) ?? new NewOrderModel();
            return order;
        }
        catch { return new NewOrderModel(); }
    }

    public async Task<bool> CreateOrder(NewOrderModel order)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            order.SL = GenerateOrderSerial.GetSL(await GetNextId());
            order.OrderDate = order.OrderDate.ToUniversalTime();
            order.DeliveryDate = order.DeliveryDate.ToUniversalTime();

            db.Orders.Add(order);
            await db.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch { return false; }
    }

    public async Task<OrderSummaryVM> GetOrderSummary()
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var orders = await db.Orders.AsNoTracking().AsSplitQuery().ToListAsync().ConfigureAwait(false);

            var utcNow = DateTime.UtcNow;
            var month = utcNow.Month;
            var year = utcNow.Year;

            var today = utcNow.Date;
            int daysSinceSunday = (int)today.DayOfWeek;
            var startOfWeek = today.AddDays(-daysSinceSunday -1);
            var endOfWeek = startOfWeek.AddDays(7); 
            var summary = new OrderSummaryVM
            {
                TotalOrders = orders.Count,
                TotalCustomers = orders.Select(o => o.MobileNumber).Distinct().Count(),
                MonthTotalOrders = orders.Count(o => o.OrderDate.Month == month && o.OrderDate.Year == year),
                WeekTotalOrders = orders.Count(o => o.OrderDate.Date >= startOfWeek && o.OrderDate.Date < endOfWeek),
                ReadyToDelivery = orders.Count(o => o.Status == OrderStatus.Completed)
            };

            return summary;
        }
        catch
        {
            return new OrderSummaryVM();
        }
    }


    public async Task<bool> UpdateOrder(NewOrderModel incoming)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var existing = await db.Orders
                .Include(o => o.PanjabiOrders)
                .Include(o => o.ArabianOrders)
                .Include(o => o.SelowerOrders)
                .FirstOrDefaultAsync(o => o.Id == incoming.Id);
            if (existing is null) return false;

            db.Entry(existing).CurrentValues.SetValues(incoming);
            existing.OrderDate = incoming.OrderDate.ToUniversalTime();
            existing.DeliveryDate = incoming.DeliveryDate.ToUniversalTime();

            SyncChildren(db, existing.ArabianOrders, incoming.ArabianOrders, (a, b) => a.Id == b.Id, e => e.OrderId = existing.Id);
            SyncChildren(db, existing.PanjabiOrders, incoming.PanjabiOrders, (a, b) => a.Id == b.Id, e => e.OrderId = existing.Id);
            SyncChildren(db, existing.SelowerOrders, incoming.SelowerOrders, (a, b) => a.Id == b.Id, e => e.OrderId = existing.Id);

            await db.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch { return false; }
    }

    private static void SyncChildren<T>(DbContext db, ICollection<T> tracked, ICollection<T>? incoming, Func<T, T, bool> match, Action<T> assignParent) where T : class
    {
        incoming ??= [];

        var toRemove = tracked.Where(t => !incoming.Any(i => match(t, i))).ToList();
        foreach (var del in toRemove) db.Remove(del);

        foreach (var inc in incoming)
        {
            var exist = tracked.FirstOrDefault(t => match(t, inc));
            if (exist is null)
            {
                assignParent(inc);
                tracked.Add(inc);
            }
            else
            {
                db.Entry(exist).CurrentValues.SetValues(inc);
            }
        }
    }

    public async Task<bool> DeleteOrder(int id)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entity = await db.Orders
                .Include(o => o.PanjabiOrders)
                .Include(o => o.ArabianOrders)
                .Include(o => o.SelowerOrders)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (entity is null) return false;
            db.Remove(entity);
            await db.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch { return false; }
    }

    public async Task<List<CustomerVM>> GetAllCustomers()
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var orders = await db.Orders.AsNoTracking().AsSplitQuery().ToListAsync().ConfigureAwait(false);
            return [.. orders
                .GroupBy(o => o.MobileNumber)
                .Select(g => g.OrderByDescending(o => o.Id).First())
                .Select(o => new CustomerVM
                {
                    CustomerName = o.CustomerName,
                    Address = o.Address,
                    MobileNumber = o.MobileNumber
                })];
        }
        catch { return []; }
    }
}
