namespace MauiApp1.Pages.Handheld;

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
    private string orderFor;

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

        //AppData.Orders.Add(newOrder);
        await Shell.Current.GoToAsync($"//orders");
    }
    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync($"//orders");
    }
}
