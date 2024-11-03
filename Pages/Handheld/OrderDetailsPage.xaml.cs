namespace MYPM.Pages.Handheld;

public partial class OrderDetailsPage : ContentPage
{
    public OrderDetailsPage(NewOrderModel order)
    {
        InitializeComponent();
        BindingContext = order;
    }
}
