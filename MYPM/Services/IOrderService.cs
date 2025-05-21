using MYPM.Data.Models;

namespace MYPM.Services;

public interface IOrderService
{
    Task<bool> CreateOrder(NewOrderModel order);
    Task<List<NewOrderModel>> GetAllOrders();
    Task<List<CustomerVM>> GetAllCustomers();
    Task<List<NewOrderModel>> GetCustomerOrders(string mobileNumber);
    Task<NewOrderModel> GetOrder(int id);
    Task<NewOrderModel> UpdateStatus(int id, OrderStatus status);
    Task<OrderSummaryVM> GetOrderSummary();
}