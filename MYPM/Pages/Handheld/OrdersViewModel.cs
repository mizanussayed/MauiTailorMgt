

namespace MYPM.Pages.Handheld;

public partial class OrdersViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

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
        var result = await App.Current.MainPage.DisplayAlert("", "Do you want to logout?", "Yes", "Ooops, no");
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
        Orders = new ObservableCollection<Order>(AppData.Orders);
    }
    [RelayCommand]
    public async Task Search(string query)
    {
        await Task.Delay(1000);
      // Orders = _orders.Contains( .Contains(query, StringComparison.CurrentCultureIgnoreCase));
    }
    public OrdersViewModel()
    {
        _orders = new ObservableCollection<Order>(AppData.Orders);
        DelayedLoad().ConfigureAwait(false);
    }

    private async Task DelayedLoad()
    {
        await Task.Delay(100);
        PageCurrentState = "Loaded";
    }
}