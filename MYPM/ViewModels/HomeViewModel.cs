using MYPM.Common;
using MYPM.Services;

namespace MYPM.ViewModels;

public partial class HomeViewModel(IOrderService orderService) : ObservableObject
{
    [ObservableProperty]
    private int _todayOrders;

    [ObservableProperty]
    private int _totalCustomer;

    [ObservableProperty]
    private int _totalOrders;

    [ObservableProperty]
    private int _monthTotalOrders;

    [ObservableProperty]
    private int _readyToDelivery;

    [ObservableProperty]
    private string _sL = string.Empty;

    [RelayCommand]
    private async Task RefreshData()
    {
        try
        {
            var summary = await orderService.GetOrderSummary();
            TotalOrders = summary.TotalOrders;
            TotalCustomer = summary.TotalCustomers;
            MonthTotalOrders = summary.MonthTotalOrders;
            TodayOrders = summary.TodayOrders;
            ReadyToDelivery = summary.ReadyToDelivery;
            SL = GenerateOrderSerial.GetSL(summary.TodayOrders + 1);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }
}
