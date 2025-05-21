using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using MYPM.Common.QRGeneration;
using MYPM.Data.Models;
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

        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Auto }, new ColumnDefinition { Width = GridLength.Star } },
            ColumnSpacing = 10,
            Padding = 10,
            HorizontalOptions = LayoutOptions.Center,
        };

        var stackLayout = new StackLayout
        {
            Children =
            {
                CreateLabel($"Order Number: {orderModel.SL}"),
                CreateLabel($"D Date: {orderModel.DeliveryDate:dd-MMM-yyyy}"),
                CreateLabel($"Customer: {orderModel.CustomerName}"),
                CreateLabel($"Mobile: {orderModel.MobileNumber}"),
                CreateLabel($"Total Amount: {orderModel.TotalAmount} BDT"),
                CreateLabel($"Advance Paid Amount: {orderModel.PaidAmount} BDT")
            }
        };

        var barcode = QrUtils.MakeQrCodeResult(CreateQRText(orderModel)).QrCode;
        barcode.WidthRequest = 100;

        grid.Add(stackLayout, 0, 0);
        grid.Add(barcode, 1, 0);

        var border = new Border
        {
            Stroke = Colors.Orange,
            Padding = new Thickness(10),
            Margin = new Thickness(0, 10),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            BackgroundColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout { header, grid }
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
                });

                Close();      
            }
        });
    }

    private static Label CreateLabel(string text) => new() { Text = text, FontSize = 12, TextColor = Colors.Black };

    private static string CreateQRText(NewOrderModel orderModel)
    {
        return $"Yousuf_Panjabi_tailor\n" +
               $"Order ID:~{orderModel.Id}~\n" +
               $"Order Number:{orderModel.SL}\n" +
               $"Date: {orderModel.DeliveryDate}\n" +
               $"Customer: {orderModel.CustomerName}\n" +
               $"Mobile: {orderModel.MobileNumber}\n" +
               $"Total: {orderModel.TotalAmount} BDT \n" +
               $"Paid: {orderModel.PaidAmount} BDT";
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
