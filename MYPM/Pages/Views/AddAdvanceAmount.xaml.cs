using MYPM.Models;
using MYPM.Services;
using MYPM.ViewModels;
using System.Windows.Input;

namespace MYPM.Pages.Views;

public partial class AddAdvanceAmount : ContentPage
{
    private readonly IBluetoothPrinterService _printerService;
    private NewOrderModel _order;
    private ICommand _saveCommand;

    public AddAdvanceAmount(NewOrderPageViewModel ctx, IBluetoothPrinterService printerService)
    {
        InitializeComponent();
        BindingContext = ctx;
        _printerService = printerService;
        _order = ctx.Order;
        _saveCommand = ctx.SaveCommand;
    }

    public AddAdvanceAmount(EditOrderPageViewModel ctx, IBluetoothPrinterService printerService)
    {
        InitializeComponent();
        BindingContext = ctx;
        _printerService = printerService;
        _order = ctx.Order;
        _saveCommand = ctx.SaveCommand;
    }

    private void PaidTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (int.TryParse(PaidEntry?.Text, out int paidAmount))
            {
                _order.PaidAmount = paidAmount;
                _order.DueAmount = _order.TotalAmount - _order.PaidAmount;
                DueLbl.Text = $"{_order.TotalAmount - _order.PaidAmount}/-";
            }
            else
            {
                _order.PaidAmount = 0;
                _order.DueAmount = _order.TotalAmount;
                DueLbl.Text = $"{_order.TotalAmount}/-";
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        _saveCommand.Execute(this);
        await Navigation.PopModalAsync();
    }

    private async void OnBackgroundTapped(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnPrintClicked(object sender, EventArgs e)
    {
        btnPrint.IsEnabled = false;

        try
        {
            _saveCommand.Execute(this);
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

            // Fast text printing
            var printed = await PrintInvoiceTextFast();

            loadingOverlay.IsVisible = false;

            if (printed)
            {
                await DisplayAlert("Success", "Invoice printed successfully!", "OK");
                await Navigation.PopModalAsync();
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
                $"Customer: {_order.CustomerName}",
                $"Mobile    : {_order.MobileNumber}",
                $"Order Type: {_order.OrderFor}",
                "",
                $"Total Amount : {_order.TotalAmount}/-",
                $"Paid Amount  : {_order.PaidAmount}/-",
                $"Due Amount   : {_order.DueAmount}/-",
                $"Delivery Date: {_order.DeliveryDate:dd MMM yyyy}",
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

    private static async Task<bool> CheckAndRequestBluetoothPermissions()
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