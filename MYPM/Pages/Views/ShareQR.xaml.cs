using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using MYPM.Common.QRGeneration;
using MYPM.Models;
using Path = System.IO.Path;

namespace MYPM.Pages.Views;

public partial class ShareQR : Popup
{
    private string _orderModelFilePath = string.Empty;
    private readonly NewOrderModel orderModel;

    public ShareQR(NewOrderModel order)
    {
        InitializeComponent();
        orderModel = order;
        _ = GenerateInvoice();
    }

    private async Task GenerateInvoice()
    {
        Label header = new() { Text = "Yousuf Tailors", FontSize = 24, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label info = new() { Text = orderModel.CustomerName, FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label infoPhone = new() { Text = orderModel.MobileNumber, FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };

        var barcode = QrUtils.MakeQrCodeResult(CreateQRText(orderModel)).QrCode;
        barcode.WidthRequest = 200;
        barcode.HeightRequest = 200;
        barcode.HorizontalOptions = LayoutOptions.End;

        var border = new Border
        {
            Stroke = Colors.Orange,
            Padding = new Thickness(2),
            Margin = new Thickness(0, 1),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            BackgroundColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout { header, info, infoPhone, barcode}
        };

        qrBox.Clear();
        qrBox.Add(border);

        await Task.Delay(300);

        await Dispatcher.DispatchAsync(async () =>
        {
            _orderModelFilePath = await SaveInvoiceAsImage(border);

            if (!string.IsNullOrEmpty(_orderModelFilePath))
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Share QR",
                    File = new ShareFile(_orderModelFilePath)
                }).ConfigureAwait(false);
            }
        });
    }

    private static string CreateQRText(NewOrderModel orderModel)
    {
        return $"Yousuf_Panjabi_tailor~{orderModel.Id}~Customer: {orderModel.CustomerName}Mobile: {orderModel.MobileNumber}";
    }
    private static async Task<string> SaveInvoiceAsImage(VisualElement visualElement)
    {
        await Task.Delay(100);
        string fileName = Path.Combine(FileSystem.CacheDirectory, "orderModel.png");

        try
        {
            var image = await visualElement.CaptureAsync();
            if (image is not null)
            {
                using var stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                await image.CopyToAsync(stream, ScreenshotFormat.Png);
                return fileName;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Capture failed: {ex.Message}");
        }

        return string.Empty;
    }
}
