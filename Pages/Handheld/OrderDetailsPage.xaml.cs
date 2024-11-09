using MYPM.Pages.Views;

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
        var action = await DisplayActionSheet("Change Order Status", "Update", "Cancel",
                                                "Pending", "Processing", "Completed", "Delivered");

        if (action is not "Cancel")
        {
            var orderStatus = Enum.Parse<OrderStatus>(action);
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
}
