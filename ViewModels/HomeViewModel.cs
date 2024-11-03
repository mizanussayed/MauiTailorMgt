using MYPM.Common;
using MYPM.Services;

namespace MYPM.ViewModels;
public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel()
    {
        RefreshData().ConfigureAwait(true);
    }
    [ObservableProperty]
    private int _todayOrders = 0;  
    [ObservableProperty]
    private int _totalCustomer = 0;
    [ObservableProperty]
    private int _totalOrders = 0;  
    [ObservableProperty]
    private int _monthTotalOrders = 0;
    [ObservableProperty]
    private int _readyToDelivary = 0;
    [ObservableProperty]
    private ObservableCollection<NewOrderModel>? _orders;

    [ObservableProperty]
    private string _nextId = string.Empty;

    private async Task RefreshData()
    {
        if (Orders is null) {

            var orderService = new OrderService();
            Orders = new ObservableCollection<NewOrderModel>(await orderService.GetAllOrders());
        }
        TotalOrders = Orders.Count;
        TotalCustomer = Orders.Select(c => c.MobileNumber).Distinct().Count();
        MonthTotalOrders = Orders.Count(o => o.OrderDate.Month == DateTime.Now.Month);
        TodayOrders = Orders.Count(o => o.OrderDate.Date == DateTime.Now.Date);
        NextId = GenerateOrderSerial.GetSL(TodayOrders + 1);
        ReadyToDelivary = Orders.Count(d => d.Status == OrderStatus.Completed);
    }
}
