using MYPM.Common.QRGeneration;

namespace MYPM.Pages.Views;
public partial class InvocieView : ContentPage
{
    private string _invoiceFilePath = string.Empty;

    public InvocieView()
    {
        InitializeComponent();
    }

    private async void OnGenerateInvoiceClicked(object sender, EventArgs e)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = "INV-12345",
            Date = DateTime.Now,
            Customer = new Customer() { Address = "123 Main St", Mobile = "01730-298184", Name = "Md Abdur Rahman ted " },
            Items =
                [
                    new() { Description = "Item 1", Quantity = 1, Price = 9.99m },
                    new() { Description = "Item 2", Quantity = 2, Price = 19.99m }
                ],
        };

        await GenerateInvoice(invoice);
    }


    private async Task GenerateInvoice(Invoice invoice)
    {
        Label header = new() { Text = "Invoice", FontSize = 24, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        var grid = new Grid()
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star },
            },
            ColumnSpacing = 10,
            Padding = 10,
            HorizontalOptions = LayoutOptions.Center,
        };

        var stactlayout = new StackLayout
        {   await CreateLabel($"Invoice Number: {invoice.InvoiceNumber}"),
            await CreateLabel($"Date: {invoice.Date:dd-MMM-yyyy}"),
            await CreateLabel($"Customer: {invoice.Customer.Name}"),
            await CreateLabel($"Mobile: {invoice.Customer.Mobile}"),
            await CreateLabel($"Total Amount: {invoice.TotalAmount} BDT")
        };
       // var barcode = QrUtils.MakeQrCode(invoice.InvoiceNumber);
        var barcode = QrUtils.MakeQrCodeResult(invoice.InvoiceNumber).QrCode;
        barcode.WidthRequest = 100;
        grid.Add(stactlayout, 0, 0);
        grid.Add(barcode, 1, 0);

        var frame = new Frame
        {
            BorderColor = Application.Current?.Resources["Primary"] as Color ?? Colors.Orange,
            Padding = new Thickness(10),
            Margin = new Thickness(0, 10),
            CornerRadius = 10,
            BackgroundColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            HasShadow = true,
            Content = new VerticalStackLayout() { header, grid }
        };

        verticalstack.Add(frame);
        _invoiceFilePath = await SaveInvoiceAsImage(frame).ConfigureAwait(false);
    }

    private static async Task<string> SaveInvoiceAsImage(VisualElement visualElement)
    {
        await Task.Delay(100);
        string fileName = Path.Combine(FileSystem.CacheDirectory, "invoice.png");
        try
        {
            var image = await visualElement.CaptureAsync();
            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            if (image is not null)
                await image.CopyToAsync(stream, ScreenshotFormat.Png);
            return await Task.FromResult(fileName);
        }
        catch (Exception)
        {
            return await Task.FromResult(string.Empty);
        }
    }


    private async void OnShareInvoiceClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_invoiceFilePath))
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Invoice",
                File = new ShareFile(_invoiceFilePath)
            }).ConfigureAwait(false);
          // File.Delete(_invoiceFilePath);
        }
        else
        {
            await DisplayActionSheet("Error", "OK", null, "No invoice to share.").ConfigureAwait(false);
        }
    }
    private static Task<Label> CreateLabel(string text)
    {
        return Task.FromResult(new Label
        {
            Text = text,
            TextColor = Colors.Black,
            FontSize = 12,
        });
    }
}
