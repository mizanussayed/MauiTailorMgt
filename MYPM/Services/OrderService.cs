using Microsoft.EntityFrameworkCore;
using MYPM.Data.Configurations;
using MYPM.Data.Models;
using MYPM.ViewModels;

namespace MYPM.Services;

public class OrderService(AppDbContext _context) : IOrderService
{
    private List<NewOrderModel>? _orderListCache = [];
    private readonly Dictionary<int, NewOrderModel> _orderDetailsCache = [];
    private OrderSummaryVM? _orderSummaryCache = null;

    private DateTime _cacheExpiry = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(2);

    public async Task<List<NewOrderModel>> GetAllOrders()
    {
        if (_orderListCache != null && DateTime.UtcNow < _cacheExpiry)
            return _orderListCache;

        try
        {
            var orders = await _context.Orders
                .OrderByDescending(o => o.Id)
                .AsNoTracking()
                .ToListAsync();

            _orderListCache = orders;
            _cacheExpiry = DateTime.UtcNow.Add(_cacheDuration);
            return orders;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return [];
        }
    }

    public async Task<List<NewOrderModel>> GetCustomerOrders(string mobileNumber)
    {
        try
        {
            var result = await _context.Orders
                .Where(c => c.MobileNumber == mobileNumber)
                .OrderByDescending(o => o.Id)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return [];
        }
    }

    public async Task<NewOrderModel> GetOrder(int id)
    {
        if (_orderDetailsCache.TryGetValue(id, out var cachedOrder))
            return cachedOrder;

        try
        {
            var order = await _context.Orders
                .Include(o => o.PanjabiOrders)
                .Include(o => o.ArabianOrders)
                .Include(o => o.SelowerOrders)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id) ?? new NewOrderModel();

            _orderDetailsCache[id] = order;
            return order;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return new NewOrderModel();
        }
    }

    public async Task<bool> CreateOrder(NewOrderModel order)
    {
        try
        {
            order.OrderDate = order.OrderDate.ToUniversalTime();
            order.DeliveryDate = order.DeliveryDate.ToUniversalTime();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Invalidate caches
            _orderListCache = null;
            _orderDetailsCache.Clear();
            _orderSummaryCache = null;

            return true;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return false;
        }
    }

    public async Task<NewOrderModel> UpdateStatus(int id, OrderStatus status)
    {
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order is null) return new NewOrderModel();

            order.Status = status;

            if (status == OrderStatus.Delivered)
            {
                order.DueAmount = 0;
                order.PaidAmount = order.TotalAmount;
            }

            await _context.SaveChangesAsync();

            // Invalidate caches
            _orderListCache = null;
            _orderDetailsCache.Remove(id);
            _orderSummaryCache = null;

            return order;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return new NewOrderModel();
        }
    }

    public async Task<OrderSummaryVM> GetOrderSummary()
    {
        if (_orderSummaryCache != null && DateTime.UtcNow < _cacheExpiry)
            return _orderSummaryCache;

        try
        {
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;
            var today = DateTime.UtcNow.Date;
            var completedStatus = (int)OrderStatus.Completed;

            var sql = @"
                SELECT 
                    COUNT(*) AS ""TotalOrders"",
                    COUNT(DISTINCT ""MobileNumber"") AS ""TotalCustomers"",
                    COUNT(*) FILTER (WHERE DATE_PART('month', ""OrderDate"") = @month AND DATE_PART('year', ""OrderDate"") = @year) AS ""MonthTotalOrders"",
                    COUNT(*) FILTER (WHERE DATE(""OrderDate"") = @today) AS ""TodayOrders"",
                    COUNT(*) FILTER (WHERE ""Status"" = @completedStatus) AS ""ReadyToDelivery""
                FROM ""Orders""";

            var result = await _context.Database.SqlQueryRaw<OrderSummaryVM>(sql,
                    new Npgsql.NpgsqlParameter("@month", month),
                    new Npgsql.NpgsqlParameter("@year", year),
                    new Npgsql.NpgsqlParameter("@today", today),
                    new Npgsql.NpgsqlParameter("@completedStatus", completedStatus))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            _orderSummaryCache = result ?? new OrderSummaryVM();
            _cacheExpiry = DateTime.UtcNow.Add(_cacheDuration);

            return _orderSummaryCache;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return new OrderSummaryVM();
        }
    }

    public async Task<List<CustomerVM>> GetAllCustomers()
    {
        try
        {
            var sql = @"
            SELECT DISTINCT ON (""MobileNumber"") 
                ""CustomerName"", 
                ""Address"", 
                ""MobileNumber""
            FROM ""Orders""
            ORDER By ""MobileNumber"", ""Id"" DESC
        ";

            var customers = await _context
                .Database
                .SqlQueryRaw<CustomerVM>(sql)
                .AsNoTracking()
                .ToListAsync();

            return customers;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return [];
        }
    }

}
