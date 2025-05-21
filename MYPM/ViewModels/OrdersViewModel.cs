using MYPM.Data.Models;
using MYPM.Pages;
using MYPM.Services;
using System.Windows.Input;

namespace MYPM.ViewModels;

public partial class OrdersViewModel(IOrderService _orderService) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<NewOrderModel>? _orders;

    [ObservableProperty]
    private NewOrderModel _newOrder = new();


    [ObservableProperty]
    bool _isRefreshing;

    [RelayCommand]
    public async Task Refresh()
    {
        await LoadFilteredOrders();
    }

    [RelayCommand]
    private async Task GetDetails(int Id)
    {
        try
        {
            NewOrder = await _orderService.GetOrder(Id);
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
    private string selectedFilter = "Week";
    public string SelectedFilter
    {
        get => selectedFilter;
        set
        {
            if (selectedFilter != value)
            {
                selectedFilter = value;
                OnPropertyChanged();
                LoadFilteredOrders().ConfigureAwait(false);
            }
        }
    }
    public ICommand FilterCommand => new Command<string>((filter) => SelectedFilter = filter);
    private async Task LoadFilteredOrders()
    {
        try
        {
            IsRefreshing = true;
            var allOrders = await _orderService.GetAllOrders().ConfigureAwait(false);
            MainThread.BeginInvokeOnMainThread(() =>
            {
            DateTime today = DateTime.Today;
            IEnumerable<NewOrderModel> filtered = selectedFilter switch
            {
                "Week" => allOrders.Where(o => o.OrderDate >= today.AddDays(-7)),
                "Month" => allOrders.Where(o => o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year),
                "Year" => allOrders.Where(o => o.OrderDate.Year == today.Year),
                "All" => allOrders,
                _ => allOrders
            };
                Orders = [.. filtered];
            });

        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}
