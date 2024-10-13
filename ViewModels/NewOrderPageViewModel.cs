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
    private async Task Save()
    {
        var newOrder = new NewOrderModel
        {
            CustomerName = Order.CustomerName,
            MobileNumber = Order.MobileNumber,
            Address = Order.Address,
            OrderDate = Order.OrderDate,
            DeliveryDate = Order.DeliveryDate
        };

        //var response = Snackbar.Make("Successfully Order Saved", null, "Ok", TimeSpan.FromSeconds(3));
       // await response.Show();
        await Shell.Current.Navigation.PopAsync();
    }
    [RelayCommand]
    private async Task Cancel()
    {
        //await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        await Shell.Current.Navigation.PopAsync();
    }
}
