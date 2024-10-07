using CommunityToolkit.Maui.Alerts;

namespace MYPM.Pages.Handheld;

public partial class NewOrderPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string customerName;

    [ObservableProperty]
    private string mobileNumber;

    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private DateTime orderDate = DateTime.Today;

    [ObservableProperty]
    private DateTime deliveryDate = DateTime.Today.AddDays(7);

    [ObservableProperty]
    private string orderFor = "Arabian";

    [RelayCommand]
    private async Task Save()
    {
        var newOrder = new NewOrder
        {
            CustomerName = CustomerName,
            MobileNumber = MobileNumber,
            Address = Address,
            OrderDate = OrderDate,
            DeliveryDate = DeliveryDate
        };

        var response = Snackbar.Make("Successfully Order Saved", null,  "Ok", TimeSpan.FromSeconds(3));
        await response.Show();
        await Shell.Current.Navigation.PopAsync();
    }
    [RelayCommand]
    private async Task Cancel()
    {
        //await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        await Shell.Current.Navigation.PopAsync();
    }
}
