using CommunityToolkit.Maui.Views;
using MYPM.Data.Models;
using MYPM.Pages.Views;
using MYPM.Services;

namespace MYPM.Pages;

[QueryProperty(nameof(Order), "Order")]
public partial class OrderDetailsPage : ContentPage
{
    private NewOrderModel? OrderModel;
    private readonly IOrderService orderService;
    public NewOrderModel? Order
    {
        get => OrderModel;
        set
        {
            OrderModel = value;
            BindingContext = OrderModel;
        }
    }

    public OrderDetailsPage(IOrderService _orderService)
    {
        InitializeComponent();
        orderService = _orderService;
    }

    private async void OnChangeStatusClicked(object sender, EventArgs e)
    {
        try
        {
            var statusValues = Enum.GetValues<OrderStatus>().ToList();
            var actionSheet = await DisplayActionSheet("Change Order Status", "Cancel", null, statusValues.Select(s => s.ToString()).ToArray());

            if (actionSheet == "Cancel") return;

            var selectedStatus = Enum.Parse<OrderStatus>(actionSheet);
            await UpdateOrderStatus(selectedStatus);
            await DisplayAlert("Status Updated", $"Order status changed to {selectedStatus}", "OK");
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }
    private void OnViewDetailsClicked(object sender, EventArgs e)
    {
        if (OrderModel?.ArabianOrders?.Count > 0)
            ArabinaOrderLayout.IsVisible = !ArabinaOrderLayout.IsVisible;

        if (OrderModel?.PanjabiOrders?.Count > 0)
            PanjabiOrderLayout.IsVisible = !PanjabiOrderLayout.IsVisible;

        if (OrderModel?.SelowerOrders?.Count > 0)
            SelowerOrderLayout.IsVisible = !SelowerOrderLayout.IsVisible;
    }
    private async Task UpdateOrderStatus(OrderStatus status)
    {
        var response = await orderService.UpdateStatus(OrderModel!.Id, status);
        BindingContext = response;
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        if (OrderModel == null)
        {
            await DisplayAlert("Error", "Order details are not available to share.", "OK");
            return;
        }

        var popup = new ShareQR(OrderModel);
        await this.ShowPopupAsync(popup);
    }

}
