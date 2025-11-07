using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using MYPM.Common.QRGeneration;
using MYPM.Models;
using MYPM.Services;
using Path = System.IO.Path;

namespace MYPM.Pages.Views;

public partial class ShareQR : Popup
{
    private string _orderModelFilePath = string.Empty;
    private readonly NewOrderModel orderModel;
    private readonly IBluetoothPrinterService _printerService;
    private Border? _invoiceBorder;

    public ShareQR(NewOrderModel order, IBluetoothPrinterService printerService)
    {
        InitializeComponent();
        orderModel = order;
        _printerService = printerService;
        _ = GenerateInvoice();
    }

    private async Task GenerateInvoice()
    {
        Label header = new() { Text = "Yousuf Tailors", FontSize = 24, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label info = new() { Text = orderModel.CustomerName, FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label tk = new() { Text = orderModel.DueAmount.ToString(), FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label infoPhone = new() { Text = orderModel.MobileNumber, FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };

        var barcode = QrUtils.MakeQrCodeResult(CreateQRText(orderModel)).QrCode;
        barcode.WidthRequest = 200;
        barcode.HeightRequest = 200;
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

        await Dispatcher.DispatchAsync(async () =>
              {
                  _orderModelFilePath = await SaveInvoiceAsImage(_invoiceBorder);

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

    private async void OnPrintClicked(object sender, EventArgs e)
    {
        btnPrint.IsEnabled = false;

        try
        {
            var devices = await _printerService.GetPairedDevicesAsync();

            if (devices.Count == 0)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlert("Error", "No paired Bluetooth devices found. Please pair your printer first.", "OK");
                return;
            }
            var selectedDevice = await Application.Current!.Windows[0].Page!.DisplayActionSheet(
                "Select Printer",
                "Cancel",
                null,
                devices.ToArray());

            if (selectedDevice == "Cancel" || string.IsNullOrEmpty(selectedDevice))
                return;

            var connectingTask = Application.Current.Windows[0].Page!.DisplayAlert("Connecting", $"Connecting to {selectedDevice}...", "Cancel");
            var connected = await _printerService.ConnectAsync(selectedDevice);

            await connectingTask;

            if (!connected)
            {
                await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Failed to connect to printer. Please make sure the printer is on and in range.", "OK");
                return;
            }

            // Print the image
            if (!string.IsNullOrEmpty(_orderModelFilePath))
            {
                var imageBytes = await File.ReadAllBytesAsync(_orderModelFilePath);
                var printed = await _printerService.PrintImageAsync(imageBytes);

                if (printed)
                {
                    await Application.Current.Windows[0].Page!.DisplayAlert("Success", "Printed successfully!", "OK");
                }
                else
                {
                    await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Failed to print. Please check the printer.", "OK");
                }
            }
            else
            {
                await Application.Current.Windows[0].Page!.DisplayAlert("Error", "No image available to print.", "OK");
            }

            await _printerService.DisconnectAsync();
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
        finally
        {
            btnPrint.IsEnabled = true;
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await CloseAsync();
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
            _ = ex.Message;
        }
        return string.Empty;
    }

    private static string CreateQRText(NewOrderModel orderModel)
    {
        return $"Yousuf_Panjabi_tailor~{orderModel.Id}~Customer: {orderModel.CustomerName}Mobile: {orderModel.MobileNumber}";
    }
}







