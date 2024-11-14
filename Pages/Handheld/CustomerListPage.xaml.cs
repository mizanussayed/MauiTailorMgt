
using MYPM.ViewModels;

namespace MYPM.Pages.Handheld;

public partial class CustomerListPage : ContentPage
{
    public CustomerListPage()
    {
        InitializeComponent();
    }


    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchText = SearchBar.Text.ToLower();
        var bc = BindingContext as OrdersViewModel;

        var filteredInvoices =  bc?.Orders?.ToList().FindAll(i => i.CustomerName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                                                       i.MobileNumber.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
        InvoiceListView.ItemsSource = filteredInvoices;
    }

    private async void OnInvoiceTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var invoice = (NewOrderModel)e.Item;
            await DisplayAlert("Invoice Details", $"Invoice Number: {invoice.CustomerName}\nCustomer: {invoice.MobileNumber}", "OK");
        }
    }
}