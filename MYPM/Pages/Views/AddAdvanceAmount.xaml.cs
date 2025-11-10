using MYPM.Services;
using MYPM.ViewModels;
using Path = System.IO.Path;
namespace MYPM.Pages.Views;

public partial class AddAdvanceAmount : ContentPage
{
    private readonly NewOrderPageViewModel context;
    private readonly IBluetoothPrinterService _printerService;
    private Border? _invoiceBorder;
    private string _invoiceImagePath = string.Empty;

    public AddAdvanceAmount(NewOrderPageViewModel ctx, IBluetoothPrinterService printerService)
    {
        InitializeComponent();
        BindingContext = ctx;
        context = ctx;
        _printerService = printerService;
    }

    private void PaidTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (int.TryParse(PaidEntry?.Text, out int paidAmount))
            {
                context.Order.PaidAmount = paidAmount;
                context.Order.DueAmount = context.Order.TotalAmount - context.Order.PaidAmount;
                DueLbl.Text = $"Due   : {context.Order.TotalAmount - context.Order.PaidAmount}";
            }
            else
            {
                context.Order.PaidAmount = 0;
                context.Order.DueAmount = context.Order.TotalAmount;
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        context.SaveCommand.Execute(this);
        await Shell.Current.Navigation.PopToRootAsync();
    }

    private async void OnPrintClicked(object sender, EventArgs e)
    {
        btnPrint.IsEnabled = false;

        try
        {
            context.SaveCommand.Execute(this);
            await Task.Delay(100);

            var permissionStatus = await CheckAndRequestBluetoothPermissions();
            if (!permissionStatus)
            {
                await DisplayAlert("Permission Required",
                 "Bluetooth permissions are required to connect to the printer. Please enable them in your device settings.",
               "OK");
                return;
            }

            var devices = await _printerService.GetPairedDevicesAsync();

            if (devices.Count == 0)
            {
                await DisplayAlert("Error", "No paired Bluetooth devices found. Please pair your printer first.", "OK");
                return;
            }

            var selectedDevice = await DisplayActionSheet("Select Printer", "Cancel", null, devices.ToArray());

            if (selectedDevice == "Cancel" || string.IsNullOrEmpty(selectedDevice))
                return;

            loadingLabel.Text = $"Connecting to {selectedDevice}...";
            loadingOverlay.IsVisible = true;
            await Task.Delay(50);

            var connected = await _printerService.ConnectAsync(selectedDevice);

            if (!connected)
            {
                loadingOverlay.IsVisible = false;
                await DisplayAlert("Error", "Failed to connect to printer.", "OK");
                return;
            }

            loadingLabel.Text = "Printing invoice...";
            await Task.Delay(50);

            // OPTION 1: Fast text printing (recommended for text-only invoices)
            var printed = await PrintInvoiceTextFast();

            // OPTION 2: Image printing (keep for complex layouts)
            // await GenerateInvoiceImage();
            // if (string.IsNullOrEmpty(_invoiceImagePath) || !File.Exists(_invoiceImagePath))
            // {
            //     loadingOverlay.IsVisible = false;
            //     await DisplayAlert("Error", "Failed to prepare invoice image for printing.", "OK");
            //     return;
            // }
            // var imageBytes = await File.ReadAllBytesAsync(_invoiceImagePath);
            // var printed = await _printerService.PrintImageAsync(imageBytes);

            loadingOverlay.IsVisible = false;

            if (printed)
            {
                await DisplayAlert("Success", "Invoice printed successfully!", "OK");
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to print. Please check the printer and try again.", "OK");
            }

            await _printerService.DisconnectAsync();
        }
        catch (Exception ex)
        {
            loadingOverlay.IsVisible = false;
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
        finally
        {
            loadingOverlay.IsVisible = false;
            btnPrint.IsEnabled = true;
        }
    }

    /// <summary>
    /// Fast text-based invoice printing (5-10x faster than image)
    /// </summary>
    private async Task<bool> PrintInvoiceTextFast()
    {
        try
        {
            var lines = new List<string>
            {
             "YOUSUF TAILOR",
             "Phone: 01730298184",
             "Brahmanbaria Hawkers Market",
             "",
             "--------------------------------",
             $"Customer: {context.Order.CustomerName}",
             $"Mobile    : {context.Order.MobileNumber}",
             $"Order Type: {context.Order.OrderFor}",
             "",
             $"Total Amount : {context.Order.TotalAmount}/-",
             $"Paid Amount  : {context.Order.PaidAmount}/-",
             $"Due Amount   : {context.Order.DueAmount}/-",
             $"Delivery Date: {context.Order.DeliveryDate:dd MMM yyyy}",
             "",
             "--------------------------------",
             "",
             "Thank you! Come again.",
             "",
             "",
             };

            return await _printerService.PrintFormattedTextAsync(lines);
        }
        catch
        {
            return false;
        }
    }

    private async Task GenerateInvoiceImage()
    {
        try
        {
            var invoiceLayout = new VerticalStackLayout
            {
                Spacing = 8,
                Padding = new Thickness(15),
                BackgroundColor = Colors.White,
                WidthRequest = 384
            };
            invoiceLayout.Add(new Label
            {
                Text = "YOUSUF TAILOR",
                FontSize = 50,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#1a1a1a")
            });


            // Contact Info
            invoiceLayout.Add(new Label
            {
                Text = "Phone: 01730298184",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#555555")
            });

            invoiceLayout.Add(new Label
            {
                Text = "Brahmanbaria Hawkers Market",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#666666"),
                Margin = new Thickness(0, 0, 0, 5)
            });

            // Separator
            invoiceLayout.Add(new BoxView
            {
                HeightRequest = 2,
                BackgroundColor = Color.FromArgb("#cccccc"),
                Margin = new Thickness(0, 5)
            });
            var customerGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowSpacing = 5,
                Margin = new Thickness(0, 10, 0, 10)
            };

            AddGridRow(customerGrid, 0, "Customer:", context.Order.CustomerName);
            AddGridRow(customerGrid, 1, "Mobile:", context.Order.MobileNumber);
            AddGridRow(customerGrid, 2, "Order Type:", context.Order.OrderFor);

            invoiceLayout.Add(customerGrid);

            // Separator
            invoiceLayout.Add(new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = Color.FromArgb("#dddddd"),
                Margin = new Thickness(0, 5)
            });

            // Amount Details Section
            var amountGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowSpacing = 8,
                Margin = new Thickness(0, 2)
            };

            AddAmountRow(amountGrid, 0, "Total Amount:", $" {context.Order.TotalAmount}/-");
            AddAmountRow(amountGrid, 1, "Paid Amount:", $" {context.Order.PaidAmount}/-");

            // Due amount with highlight
            amountGrid.Add(new Label
            {
                Text = "Due Amount:",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#d32f2f")
            }, 0, 2);

            amountGrid.Add(new Label
            {
                Text = $"{context.Order.DueAmount}/-",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.End,
                TextColor = Color.FromArgb("#d32f2f")
            }, 1, 2);

            invoiceLayout.Add(amountGrid);

            invoiceLayout.Add(new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = Color.FromArgb("#dddddd"),
                Margin = new Thickness(0, 5)
            });

            // Dates Section
            var datesGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowSpacing = 5,
                Margin = new Thickness(0, 2)
            };
            AddGridRow(datesGrid, 1, "Delivery Date:", context.Order.DeliveryDate.ToString("dd MMM yyyy"));

            invoiceLayout.Add(datesGrid);

            // Separator
            invoiceLayout.Add(new BoxView
            {
                HeightRequest = 2,
                BackgroundColor = Color.FromArgb("#cccccc"),
                Margin = new Thickness(0, 10, 0, 5)
            });

            // Footer
            invoiceLayout.Add(new Label
            {
                Text = "Thank you! Come again.",
                FontSize = 25,
                FontAttributes = FontAttributes.Italic,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#666666"),
                Margin = new Thickness(0, 5, 0, 0)
            });

            _invoiceBorder = new Border
            {
                Stroke = Color.FromArgb("#e0e0e0"),
                StrokeThickness = 1,
                Padding = new Thickness(0),
                BackgroundColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                Content = invoiceLayout
            };

            var tempContainer = new AbsoluteLayout
            {
                IsVisible = true,
                Opacity = 0,
                Children = { _invoiceBorder }
            };

            if (Content is Grid mainGrid)
            {
                mainGrid.Children.Add(tempContainer);

                await Dispatcher.DispatchAsync(async () =>
                {
                    await Task.Delay(200);
                });
            }

            await Task.Delay(1000);

            if (_invoiceBorder.Width <= 0 || _invoiceBorder.Height <= 0)
            {
                var size = _invoiceBorder.Measure(384, double.PositiveInfinity);
                _invoiceBorder.Arrange(new Rect(0, 0, size.Width, size.Height));
                await Task.Delay(200);
            }

            _invoiceImagePath = await SaveInvoiceAsImage(_invoiceBorder);

            if (Content is Grid grid && grid.Children.Contains(tempContainer))
            {
                grid.Children.Remove(tempContainer);
            }

            if (!string.IsNullOrEmpty(_invoiceImagePath) && File.Exists(_invoiceImagePath))
            {
                var fileInfo = new FileInfo(_invoiceImagePath);
            }
        }
        catch
        {
            
        }
    }

    private static void AddGridRow(Grid grid, int row, string label, string value)
    {
        grid.Add(new Label
        {
            Text = label,
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#333333"),
            Margin = new Thickness(0, 0, 10, 0)
        }, 0, row);

        grid.Add(new Label
        {
            Text = value,
            FontSize = 28,
            TextColor = Color.FromArgb("#555555")
        }, 1, row);
    }

    private void AddAmountRow(Grid grid, int row, string label, string value)
    {
        grid.Add(new Label
        {
            Text = label,
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#333333")
        }, 0, row);

        grid.Add(new Label
        {
            Text = value,
            FontSize = 28,
            HorizontalOptions = LayoutOptions.End,
            TextColor = Color.FromArgb("#333333")
        }, 1, row);
    }

    private static async Task<string> SaveInvoiceAsImage(VisualElement visualElement)
    {
        await Task.Delay(100);
        string fileName = Path.Combine(FileSystem.CacheDirectory, "orderModel.png");

        try
        {

            if (visualElement.Width <= 0 || visualElement.Height <= 0)
            {
                return string.Empty;
            }

            var image = await visualElement.CaptureAsync();

            if (image is null)
            {
                return string.Empty;
            }
            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            await image.CopyToAsync(stream, ScreenshotFormat.Png);
            await stream.FlushAsync();

            return fileName;
        }
        catch
        {
            return string.Empty;
        }
    }

    private async Task<bool> CheckAndRequestBluetoothPermissions()
    {
#if ANDROID
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
        {
            var connectStatus = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (connectStatus != PermissionStatus.Granted)
            {
                connectStatus = await Permissions.RequestAsync<Permissions.Bluetooth>();
                if (connectStatus != PermissionStatus.Granted)
                    return false;
            }
        }
        else
        {
            var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (locationStatus != PermissionStatus.Granted)
            {
                locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (locationStatus != PermissionStatus.Granted)
                    return false;
            }
        }
#endif
        return true;
    }
}
