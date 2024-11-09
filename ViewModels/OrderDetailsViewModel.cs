using MYPM.Pages.Handheld;

namespace MYPM.ViewModels;

[QueryProperty("Order", "Order")]

public partial class OrderDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    NewOrderModel order;

    [ObservableProperty]
    Item? added;

    [RelayCommand]
    async Task Pay()
    {
        try
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Order", Order }
            };
            await Shell.Current.GoToAsync($"{nameof(OrdersPage)}", navigationParameter);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
