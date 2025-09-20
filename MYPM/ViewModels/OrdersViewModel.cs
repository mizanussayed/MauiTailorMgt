using Java.Time;
using Microsoft.Maui.Controls;
using MYPM.Models;
using MYPM.Pages;
using MYPM.Services;
using System.Windows.Input;

namespace MYPM.ViewModels;

public partial class OrdersViewModel(IOrderService _orderService) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<NewOrderModel>? _orders;

    [ObservableProperty]
    private NewOrderModel? _newOrder;


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
            var navigationParameter = new Dictionary<string, object>
            {
                { "OrderId", Id }
            };
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    public async Task<NewOrderModel> LoadOrderById(int id)
    {
        try
        {
            return await _orderService.GetOrder(id).ConfigureAwait(false);
        }
        catch (Exception)
        {
           return NewOrder!;
        }
    }

    public async Task<bool> Delete(int Id)
    {
        try
        {
            return await _orderService.DeleteOrder(Id).ConfigureAwait(true);
        }
        catch (Exception)
        {
          return false;
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

            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DateTime today = DateTime.Today;
                IEnumerable<NewOrderModel> filtered = selectedFilter switch
                {
                    "Week" => allOrders.Where(o => o.OrderDate >= startOfWeek && o.OrderDate < endOfWeek),
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
