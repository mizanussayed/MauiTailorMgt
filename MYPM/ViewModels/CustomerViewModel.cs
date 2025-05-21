using MYPM.Data.Models;
using MYPM.Pages;
using MYPM.Services;

namespace MYPM.ViewModels;


[QueryProperty(nameof(MobileNumber), nameof(MobileNumber))]
public partial class CustomerViewModel(IOrderService _orderService) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<NewOrderModel>? _orders;


    [ObservableProperty]
    private ObservableCollection<CustomerVM>? _customers;


    [ObservableProperty]
    string mobileNumber = string.Empty;


    [RelayCommand]
    private async Task GetDetails(int Id)
    {
        try
        {
            var data = await _orderService.GetOrder(Id);
            var navigationParameter = new Dictionary<string, object>
            {
                { "Order", data }
            };
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    [RelayCommand]
    private async Task LoadFilteredOrders()
    {
        try
        {
            var customers = await _orderService.GetAllCustomers().ConfigureAwait(false);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Customers = [.. customers];
            });

        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }


    [RelayCommand]
    private async Task LoadCustomerOrders()
    {
        try
        {
            var data = await _orderService.GetCustomerOrders(MobileNumber);
            MainThread.BeginInvokeOnMainThread(() => {
                Orders = [.. data];
            });
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }
}
