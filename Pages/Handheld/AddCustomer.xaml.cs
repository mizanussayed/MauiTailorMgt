namespace MYPM.Pages.Handheld;

public partial class AddCustomer : ContentPage
{
    private List<InvoiceItem> _invoiceItems = new List<InvoiceItem>();
    private List<Invoice> _invoices = new List<Invoice>();
    private int _invoiceCount = 0;

    public AddCustomer()
    {
        InitializeComponent();
    }
    private async void OnCreateInvoiceClicked(object sender, EventArgs e)
    {

        await Task.Delay(0);
        var invoice = new Invoice
        {
            InvoiceNumber = (++_invoiceCount).ToString(),
            Date = DateTime.Now,
            Items = new List<InvoiceItem>(_invoiceItems),
            TotalAmount = _invoiceItems.Sum(i => i.Price * i.Quantity)
        };

        Dispatcher.Dispatch(async () =>
        {
            _invoices.Add(invoice);
            await DisplayAlert("Invoice Created", $"Invoice #{invoice.InvoiceNumber} created!", "OK");
            _invoiceItems.Clear();
        });

    }


    private async void OnViewInvoicesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InvoiceListPage(_invoices));
    }

    private string CreateInvoiceText(Invoice invoice)
    {
        return $"Invoice Number: {invoice.InvoiceNumber}\n" +
               $"Date: {invoice.Date:d}\n" +
               $"Customer: {invoice.Customer.Name}\n" +
               $"Total: {invoice.TotalAmount} BDT";
    }
}