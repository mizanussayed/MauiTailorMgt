using MYPM.Data.Models;
using MYPM.Services;

namespace MYPM.ViewModels;

public partial class NewOrderPageViewModel(IOrderService orderService) : ObservableObject
{
    [ObservableProperty]
    private NewOrderModel order = new();

    [ObservableProperty]
    private ArabianOrder? arabianOrder;

    [ObservableProperty]
    private PanjabiOrder? panjabiOrder;

    [ObservableProperty]
    private SelowerOrder? selowerOrder;

    [RelayCommand]
    private async Task<bool> Save()
    {
        if (ArabianOrder is not null) Order.ArabianOrders.Add(ArabianOrder);
        if (PanjabiOrder is not null) Order.PanjabiOrders.Add(PanjabiOrder);
        if (SelowerOrder is not null) Order.SelowerOrders.Add(SelowerOrder);

        var result = await orderService.CreateOrder(Order);
        return result;
    }
    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
