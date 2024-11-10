using MYPM.Pages.Views;
using MYPM.Services;

namespace MYPM.Pages.Handheld;

public partial class OrderDetailsPage : ContentPage
{
    private readonly NewOrderModel OrderModel;
    public OrderDetailsPage(NewOrderModel order)
    {
        InitializeComponent();
        BindingContext = order;
        OrderModel = order;
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new InvoiceQR(OrderModel));
    }
    private async void OnChangeStatusClicked(object sender, EventArgs e)
    {
        try
        {
            var action = await DisplayActionSheet("Change Order Status", "Update", "Cancel",
                                        "Pending", "Processing", "Completed", "Delivered");
            if (action is not null && action != "Cancel")
            {
                var orderStatus = Enum.Parse<OrderStatus>(action);
                await UpdateOrderStatus(orderStatus);
                await DisplayAlert("Status Updated", $"Order status changed to {orderStatus}", "OK");
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }
    private void OnViewDetailsClicked(object sender, EventArgs e)
    {
        if (OrderModel?.ArabianOrder?.Amount > 0)
            ArabinaOrderLayout.IsVisible = !ArabinaOrderLayout.IsVisible;

        if (OrderModel?.PanjabiOrder?.Amount > 0)
            PanjabiOrderLayout.IsVisible = !PanjabiOrderLayout.IsVisible;

        if (OrderModel?.SelowerOrder?.Amount > 0)
            SelowerOrderLayout.IsVisible = !SelowerOrderLayout.IsVisible;
    }
    private async Task UpdateOrderStatus(OrderStatus status)
    {
        var service = new OrderService();
        var response = await service.UpdateStatus(OrderModel.Id, status);
        BindingContext = response;
    }
}
