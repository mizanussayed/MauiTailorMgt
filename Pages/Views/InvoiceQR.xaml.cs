using MYPM.Common.QRGeneration;

namespace MYPM.Pages.Views;
public partial class InvoiceQR : ContentPage
{
    private string _invoiceFilePath = string.Empty;

    public InvoiceQR(NewOrderModel invoice)
    {
        InitializeComponent();
       GenerateInvoice(invoice).ConfigureAwait(true);
    }


    private async Task GenerateInvoice(NewOrderModel invoice)
    {
        Label header = new() { Text = "Yousuf Tailors", FontSize = 24, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
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
        {   await CreateLabel($"Invoice Number: {invoice.Id}"),
            await CreateLabel($"Date: {invoice.DeliveryDate:dd-MMM-yyyy}"),
            await CreateLabel($"Customer: {invoice.CustomerName}"),
            await CreateLabel($"Mobile: {invoice.MobileNumber}"),
            await CreateLabel($"Total Amount: {invoice.TotalAmount} BDT")
        };

        var barcode = QrUtils.MakeQrCodeResult(createQRText(invoice)).QrCode;
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

        qrBox.Add(frame);
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

    private string createQRText(NewOrderModel invoice)
    {
        return $"Invoice Number: {invoice.Id}\n" +
               $"Date: {invoice.DeliveryDate:d}\n" +
               $"Customer: {invoice.CustomerName}\n" +
               $"Mobile: {invoice.MobileNumber}\n" +
               $"Total: {invoice.TotalAmount} BDT";
    }
}
