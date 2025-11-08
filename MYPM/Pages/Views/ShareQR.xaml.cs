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
        try
        {
            // Check and request Bluetooth permissions
            var permissionStatus = await CheckAndRequestBluetoothPermissions();
            if (!permissionStatus)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlert("Permission Required",
                         "Bluetooth permissions are required to connect to the printer. Please enable them in your device settings.",
                    "OK");
                return;
            }

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

            // Ensure image is generated
            if (string.IsNullOrEmpty(_orderModelFilePath) || !File.Exists(_orderModelFilePath))
            {
                System.Diagnostics.Debug.WriteLine("Image file not found, regenerating...");
                loadingLabel.Text = "Preparing image...";
                loadingOverlay.IsVisible = true;
                await Task.Delay(50); // Reduced delay

                _orderModelFilePath = await SaveInvoiceAsImage(_invoiceBorder);

                if (string.IsNullOrEmpty(_orderModelFilePath))
                {
                    loadingOverlay.IsVisible = false;
                    await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Failed to prepare image for printing.", "OK");
                    return;
                }
            }

            // Show loading indicator
            loadingLabel.Text = $"Connecting to {selectedDevice}...";
            loadingOverlay.IsVisible = true;

            // Small delay to ensure UI updates
            await Task.Delay(50); // Reduced from 100ms

            var connected = await _printerService.ConnectAsync(selectedDevice);

            if (!connected)
            {
                loadingOverlay.IsVisible = false;
                await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Failed to connect to printer. Please make sure the printer is on and in range.", "OK");
                return;
            }

            // Show printing progress with better message
            loadingLabel.Text = "Sending to printer...";
            await Task.Delay(50); // Reduced from 100ms

            // Read and validate image
            var imageBytes = await File.ReadAllBytesAsync(_orderModelFilePath);
            System.Diagnostics.Debug.WriteLine($"Image file size: {imageBytes.Length} bytes");

            if (imageBytes.Length == 0)
            {
                loadingOverlay.IsVisible = false;
                await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Image file is empty.", "OK");
                return;
            }

            // Print the image - this is now much faster with optimized chunking
            var printStartTime = DateTime.Now;
            var printed = await _printerService.PrintImageAsync(imageBytes);
            var printDuration = (DateTime.Now - printStartTime).TotalSeconds;

            // Hide loading indicator
            loadingOverlay.IsVisible = false;

            if (printed)
            {
                System.Diagnostics.Debug.WriteLine($"Print completed in {printDuration:F2} seconds");
                await Application.Current.Windows[0].Page!.DisplayAlert("Success", "Printed successfully!", "OK");
            }
            else
            {
                await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Failed to print. Please check the printer and try again.", "OK");
            }

            await _printerService.DisconnectAsync();
        }
        catch (Exception ex)
        {
            loadingOverlay.IsVisible = false;
            System.Diagnostics.Debug.WriteLine($"Print error: {ex.Message}\n{ex.StackTrace}");
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
        finally
        {
            loadingOverlay.IsVisible = false;
        }
    }

    private async Task<bool> CheckAndRequestBluetoothPermissions()
    {
#if ANDROID
        // Check Android version - Android 12+ requires BLUETOOTH_CONNECT and BLUETOOTH_SCAN
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
        {
            var connectStatus = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

            if (connectStatus != PermissionStatus.Granted)
            {
                connectStatus = await Permissions.RequestAsync<Permissions.Bluetooth>();

                if (connectStatus != PermissionStatus.Granted)
                {
                    return false;
                }
            }
        }
        else
        {
            // For Android 11 and below, check location permissions
            var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (locationStatus != PermissionStatus.Granted)
            {
                locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (locationStatus != PermissionStatus.Granted)
                {
                    return false;
                }
            }
        }
#endif
        return true;
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
