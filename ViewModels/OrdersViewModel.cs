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
    private ObservableCollection<NewOrderModel> _orders;

    [ObservableProperty]
    private string? displayName;

    [ObservableProperty]
    private string? displayEmail;

    [ObservableProperty]
    private ImageSource? profilePhoto;

    [ObservableProperty]
    private string? pageCurrentState = "Loading";

    [RelayCommand]
    public async Task LogOut()
    {
        var result = await Application.Current.MainPage.DisplayAlert("", "Do you want to logout?", "Yes", "Ooops, no");
        if (!result)
            return;

        await Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    public async Task GoToSetting()
    {
        await Shell.Current.GoToAsync($"//settings");
    }
    [RelayCommand]
    public async Task Refresh()
    {
        await Task.Delay(1000);
        var orderService = new OrderService();
        var dd = orderService.GetAllOrders().Result;
        Orders = new ObservableCollection<NewOrderModel>(dd);
    }
    [RelayCommand]
    public async Task Search(string query)
    {
        await Task.Delay(1000);
        // Orders = _orders.Contains( .Contains(query, StringComparison.CurrentCultureIgnoreCase));
    }


    private async Task DelayedLoad()
    {

        var orderService = new OrderService();
        var dd = await orderService.GetAllOrders();
        Orders = new ObservableCollection<NewOrderModel>(dd);
        await Task.Delay(100);
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
}