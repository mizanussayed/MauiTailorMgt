using CommunityToolkit.Maui.Extensions;
using MYPM.Models;
using MYPM.Pages.Views;
using MYPM.ViewModels;

namespace MYPM.Pages;

[QueryProperty(nameof(OrderId), "OrderId")]
public partial class OrderDetailsPage : ContentPage
{
    private int NewId;
    private NewOrderModel? OrderModel;
    private readonly OrdersViewModel _orderViewModel;
    public int OrderId
    {
        get => NewId;
        set
        {
            NewId = value;
            _ = LoadData();
        }
    }

    public OrderDetailsPage(OrdersViewModel viewModel)
    {
        InitializeComponent();
        _orderViewModel = viewModel;
    }

    private async Task LoadData()
    {
        OrderModel = await _orderViewModel.LoadOrderById(NewId);
        BindingContext = OrderModel;

        if (OrderModel?.ArabianOrders?.Count > 0)
            ArabianOrderLayout.IsVisible = true;

        if (OrderModel?.PanjabiOrders?.Count > 0)
            PanjabiOrderLayout.IsVisible = true;

        if (OrderModel?.SelowerOrders?.Count > 0)
            SelowerOrderLayout.IsVisible = true;

    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        if (OrderModel == null)
        {
            await DisplayAlert("Error", "Order details are not available to share.", "OK");
            return;
        }
        var popup = new ShareQR(OrderModel);
        await this.ShowPopupAsync(popup).ConfigureAwait(false);
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (OrderModel is null) return;
        var nav = new Dictionary<string, object> { ["Order"] = OrderModel };
        await Shell.Current.GoToAsync(nameof(EditOrderPage), nav);
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (OrderModel is null) return;
        var confirm = await DisplayAlert("Delete", $"Delete order {OrderModel.SL}?", "Yes", "No");
        if (!confirm) return;
         var isDeleted =  await _orderViewModel.Delete(OrderModel.Id);

        if (isDeleted)
        {
            await _orderViewModel.RefreshCommand.ExecuteAsync(null);
            await Shell.Current.Navigation.PopToRootAsync();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to delete order.", "OK");
        }
    }
}
