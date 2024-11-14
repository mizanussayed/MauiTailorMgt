using CommunityToolkit.Maui.Views;
using MYPM.Pages.Views;
using MYPM.Services;

namespace MYPM.Pages.Handheld;

[QueryProperty(nameof(Order), "Order")]
public partial class OrderDetailsPage : ContentPage
{
    private NewOrderModel? OrderModel;

    public NewOrderModel? Order
    {
        get => OrderModel;
        set
        {
            OrderModel = value;
            BindingContext = OrderModel;
        }
    }

    public OrderDetailsPage()
    {
        InitializeComponent();
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new InvoiceQR(OrderModel!));
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
        var response = await service.UpdateStatus(OrderModel!.Id, status);
        BindingContext = response;
    }
}
