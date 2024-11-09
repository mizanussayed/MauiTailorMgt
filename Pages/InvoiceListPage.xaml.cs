using MYPM.Pages.Views;

namespace MYPM.Pages;

public partial class InvoiceListPage : ContentPage
{
    private List<Invoice> _invoices;

    public InvoiceListPage(List<Invoice> invoices)
    {
        InitializeComponent();
        _invoices = invoices;
        InvoiceListView.ItemsSource = _invoices;
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchText = SearchBar.Text.ToLower();
        var filteredInvoices = _invoices.FindAll(i => i.InvoiceNumber.ToLower().Contains(searchText) ||
                                                       i.Customer.Name.ToLower().Contains(searchText));
        InvoiceListView.ItemsSource = filteredInvoices;
    }

    private async void OnInvoiceTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var invoice = (Invoice)e.Item;
            await DisplayAlert("Invoice Details", $"Invoice Number: {invoice.InvoiceNumber}\nCustomer: {invoice.Customer.Name}", "OK");
        }
    }

    private async void OnScanQRCodeClicked(object sender, EventArgs e)
    {
        var vw = new QRScreenerView();
        await Shell.Current.Navigation.PushAsync(vw, true);
    }
}