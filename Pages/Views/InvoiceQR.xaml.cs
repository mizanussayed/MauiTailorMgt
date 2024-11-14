using Microsoft.Maui.Controls.Shapes;
using MYPM.Common.QRGeneration;
using MYPM.Pages.Handheld;
using Path = System.IO.Path;

namespace MYPM.Pages.Views;
public partial class InvoiceQR : ContentPage
{
    private string _orderModelFilePath = string.Empty;
    private readonly NewOrderModel? orderModel;
    public InvoiceQR(NewOrderModel order)
    {
        InitializeComponent();
        orderModel = order;
        GenerateInvoice().ConfigureAwait(true);
    }


    private async Task GenerateInvoice()
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
        {   await CreateLabel($"Order Number: {orderModel!.Id}"),
            await CreateLabel($"Date: {orderModel.DeliveryDate:dd-MMM-yyyy}"),
            await CreateLabel($"Customer: {orderModel.CustomerName}"),
            await CreateLabel($"Mobile: {orderModel.MobileNumber}"),
            await CreateLabel($"Total Amount: {orderModel.TotalAmount} BDT")
        };

        var barcode = QrUtils.MakeQrCodeResult(CreateQRText(orderModel)).QrCode;
        barcode.WidthRequest = 100;
        grid.Add(stactlayout, 0, 0);
        grid.Add(barcode, 1, 0);

        var border = new Border
        {

            Stroke = Application.Current?.Resources["Primary"] as Color ?? Colors.Orange,
            Padding = new Thickness(10),
            Margin = new Thickness(0, 10),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            BackgroundColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout() { header, grid }
        };

        qrBox.Add(border);
        _orderModelFilePath = await SaveInvoiceAsImage(border).ConfigureAwait(false);
    }

    private static async Task<string> SaveInvoiceAsImage(VisualElement visualElement)
    {
        await Task.Delay(100);
        string fileName = Path.Combine(FileSystem.CacheDirectory, "orderModel.png");
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
        if (!string.IsNullOrEmpty(_orderModelFilePath))
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Invoice",
                File = new ShareFile(_orderModelFilePath)
            }).ConfigureAwait(false);
        }
        else
        {
            await DisplayActionSheet("Error", "OK", null, "No orderModel to share.").ConfigureAwait(false);
        }
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
          { "Order", orderModel! }
        };
        await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
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

    private static string CreateQRText(NewOrderModel orderModel)
    {
        return $"Yousuf_Panjabi_tailor\n" +
               $"Order Number:~{orderModel.Id}~\n" +
               $"Date: {orderModel.DeliveryDate:d}\n" +
               $"Customer: {orderModel.CustomerName}\n" +
               $"Mobile: {orderModel.MobileNumber}\n" +
               $"Total: {orderModel.TotalAmount} BDT";
    }
}
