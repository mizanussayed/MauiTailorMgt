using Microsoft.Maui.Controls.Shapes;
using MYPM.Common.QRGeneration;
using MYPM.Models;
using Path = System.IO.Path;

namespace MYPM.Pages.Views;

public partial class ShareQR : ContentPage
{
    private string _orderModelFilePath = string.Empty;
    private readonly NewOrderModel orderModel;
    private Border? _invoiceBorder;

    public ShareQR(NewOrderModel order)
    {
        InitializeComponent();
        orderModel = order;
        _ = GenerateInvoice();
    }

    private async Task GenerateInvoice()
    {
        Label header = new()
        {
            Text = "Yousuf Tailors",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };

        Label info = new()
        {
            Text = orderModel.CustomerName,
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };

        Label tk = new()
        {
            Text = orderModel.DueAmount.ToString(),
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };

        Label infoPhone = new()
        {
            Text = orderModel.MobileNumber,
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };

        var barcode = QrUtils.MakeQrCodeResult(CreateQRText(orderModel)).QrCode;
        barcode.WidthRequest = 250;
        barcode.HeightRequest = 250;
        barcode.HorizontalOptions = LayoutOptions.Center;

        _invoiceBorder = new Border
        {
            Stroke = Colors.Green,
            Padding = new Thickness(2),
            Margin = new Thickness(0, 10),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            BackgroundColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout { header, info, infoPhone, tk, barcode }
        };

        qrBox.Clear();
        qrBox.Add(_invoiceBorder);

        await Task.Delay(300);

        // Pre-generate the image for sharing
        _orderModelFilePath = await SaveInvoiceAsImage(_invoiceBorder);
        try
        {
            loadingLabel.Text = "Preparing to share...";
            loadingOverlay.IsVisible = true;

            if (string.IsNullOrEmpty(_orderModelFilePath) || !File.Exists(_orderModelFilePath))
            {
                _orderModelFilePath = await SaveInvoiceAsImage(_invoiceBorder!);
            }

            if (!string.IsNullOrEmpty(_orderModelFilePath))
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Share QR Code",
                    File = new ShareFile(_orderModelFilePath)
                });
            }
            else
            {
                await DisplayAlert("Error", "Failed to prepare QR code for sharing.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to share: {ex.Message}", "OK");
        }
        finally
        {
            loadingOverlay.IsVisible = false;
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private static async Task<string> SaveInvoiceAsImage(VisualElement visualElement)
    {
        await Task.Delay(100);
        string fileName = Path.Combine(FileSystem.CacheDirectory, "orderQR.png");

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
        catch{     
        }
        return string.Empty;
    }

    private static string CreateQRText(NewOrderModel orderModel)
    {
        return $"Yousuf_Panjabi_tailor~{orderModel.Id}~Customer: {orderModel.CustomerName}Mobile: {orderModel.MobileNumber}";
    }
}