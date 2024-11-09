using MYPM.Services;

namespace MYPM.ViewModels;

public partial class NewOrderPageViewModel : ObservableObject
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
        var orderService = new OrderService();
        return await orderService.CreateOrder(Order, ArabianOrder, PanjabiOrder, SelowerOrder);
    }
    [RelayCommand]
    private async Task Cancel()
    {
        //await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        await Shell.Current.Navigation.PopAsync();
    }
}
