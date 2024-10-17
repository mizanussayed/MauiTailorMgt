using MYPM.Pages.Handheld;
using MYPM.Pages.Views;

namespace MYPM.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void AddNewOrderTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewOrderPage));
    }
    private async void AddNewCustomerTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddCustomer));
    }

    private async void AssignOrderTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewOrderListPage));
    }

    private async void ViewInvoicesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InvoiceListPage));
    }
}