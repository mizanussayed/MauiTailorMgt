using MYPM.Pages.Handheld;
using MYPM.Services;

namespace MYPM.ViewModels;

public partial class OrdersViewModel : ObservableObject
{
    public OrdersViewModel()
    {
        DelayedLoad().ConfigureAwait(false);
    }

    [ObservableProperty]
    private ObservableCollection<NewOrderModel>? _orders;

    [ObservableProperty]
    private NewOrderModel _newOrder = new();

    [ObservableProperty]
    private string? pageCurrentState = "Loading";

    [RelayCommand]
    private static async Task LogOut()
    {
        var result = await Shell.Current.DisplayAlert("", "Do you want to logout?", "Yes", "Ooops, no");
        if (!result)
            return;

        await Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    public async Task Refresh()
    {
        var orderService = new OrderService();
        Orders = new ObservableCollection<NewOrderModel>(await orderService.GetAllOrders());
    }

    private async Task DelayedLoad()
    {
        if (Orders is null)
        {
            var orderService = new OrderService();
            Orders = new ObservableCollection<NewOrderModel>(await orderService.GetAllOrders());
        }
        PageCurrentState = "Loaded";
    }

    private static readonly Random _random = new Random();

    private static readonly string[] brushes = ["#FFB572", "#65B0F6", "#FF7CA3", "#50D1AA", "#9290FE"];
    public static Brush RandomBrush
    {
        get
        {
            var id = _random.Next(0, 4);
            return new SolidColorBrush(Color.Parse(brushes[id]));
        }
    }

    [RelayCommand]
    private async Task Pay()
    {
        try
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Order", this }
            };
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private async Task GetDetails(string Id)
    {
        try
        {
            var orderService = new OrderService();
            NewOrder = await orderService.GetOrder(Id);
            var navigationParameter = new Dictionary<string, object>
            {
                { "Order", NewOrder }
            };
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
