using MYPM.Pages.Handheld;

namespace MYPM.ViewModels;

[QueryProperty("Order", "Order")]
[QueryProperty("Added", "Added")]
public partial class OrderDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    Order order = new();

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
