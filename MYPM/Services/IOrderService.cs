using MYPM.Models;

namespace MYPM.Services;

public interface IOrderService
{
    Task<bool> CreateOrder(NewOrderModel order);
    Task<List<NewOrderModel>> GetAllOrders();
    Task<List<CustomerVM>> GetAllCustomers();
    Task<List<NewOrderModel>> GetCustomerOrders(string mobileNumber);
    Task<NewOrderModel> GetOrder(int id);
    Task<OrderSummaryVM> GetOrderSummary();
    Task<bool> UpdateOrder(NewOrderModel order);
    Task<bool> DeleteOrder(int id);
}