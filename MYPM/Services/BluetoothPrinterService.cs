using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Printing;
namespace MYPM.Services;

public class BluetoothPrinterService : IBluetoothPrinterService
{
    private IBluetoothLE _ble;
    private IAdapter _adapter;
    private IDevice? _connectedDevice;
    private ICharacteristic? _writeCharacteristic;

    // Common ESC/POS printer service UUID
    private readonly Guid _printerServiceUuid = Guid.Parse("000018f0-0000-1000-8000-00805f9b34fb");
    private readonly Guid _writeCharacteristicUuid = Guid.Parse("00002af1-0000-1000-8000-00805f9b34fb");

    public BluetoothPrinterService()
    {
        _ble = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;
    }

    public async Task<List<string>> GetPairedDevicesAsync()
    {
        var devices = new List<string>();

        try
        {
            var systemDevices = _adapter.GetSystemConnectedOrPairedDevices();
            devices.AddRange(systemDevices.Select(d => d.Name ?? "Unknown Device"));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting paired devices: {ex.Message}");
        }

        return await Task.FromResult(devices);
    }

    public async Task<bool> ConnectAsync(string deviceName)
    {
        try
        {
            if (!_ble.IsOn)
            {
                if (Application.Current!.Windows[0].Page != null)
                {
                    await Application.Current.Windows[0].Page!.DisplayAlert("Error", "Bluetooth is turned off", "OK");
                }
                return false;
            }

            var devices = _adapter.GetSystemConnectedOrPairedDevices();
            _connectedDevice = devices.FirstOrDefault(d => d.Name == deviceName);

            if (_connectedDevice == null)
            {
                _adapter.ScanTimeout = 10000;
                _adapter.DeviceDiscovered += (s, a) =>
              {
                  if (a.Device.Name == deviceName)
                      _connectedDevice = a.Device;
              };

                await _adapter.StartScanningForDevicesAsync();
                await _adapter.StopScanningForDevicesAsync();
            }

            if (_connectedDevice == null)
                return false;

            await _adapter.ConnectToDeviceAsync(_connectedDevice);

            var service = await _connectedDevice.GetServiceAsync(_printerServiceUuid);
            if (service == null)
            {
                var services = await _connectedDevice.GetServicesAsync();
                service = services.FirstOrDefault();
            }

            if (service != null)
            {
                var characteristics = await service.GetCharacteristicsAsync();
                _writeCharacteristic = characteristics.FirstOrDefault(c =>
                   c.CanWrite || c.Id == _writeCharacteristicUuid);
            }

            return _writeCharacteristic != null;
        }
        catch (DeviceConnectionException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Connection error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> PrintImageAsync(byte[] imageData)
    {
        if (_connectedDevice == null || _writeCharacteristic == null)
            return false;

        try
        {
            // Initialize printer
            await _writeCharacteristic.WriteAsync(EscPosCommands.Initialize());
            await Task.Delay(100);

            // Center align
            await _writeCharacteristic.WriteAsync(EscPosCommands.CenterAlign());
            await Task.Delay(50);
            var (processedData, width, height) = ImageProcessor.ProcessImage(imageData, 384);

            if (processedData.Length > 0)
            {
                var imageCommand = EscPosCommands.PrintImage(processedData, width, height);
                await _writeCharacteristic.WriteAsync(imageCommand);
                await Task.Delay(200);
            }

            // Feed lines and cut
            await _writeCharacteristic.WriteAsync(EscPosCommands.FeedLines(3));
            await Task.Delay(100);

            await _writeCharacteristic.WriteAsync(EscPosCommands.FullCut());
            await Task.Delay(100);

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Print error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> PrintTextAsync(string text)
    {
        if (_connectedDevice == null || _writeCharacteristic == null)
            return false;

        try
        {
            // Initialize printer
            await _writeCharacteristic.WriteAsync(EscPosCommands.Initialize());
            await Task.Delay(50);

            // Center align
            await _writeCharacteristic.WriteAsync(EscPosCommands.CenterAlign());
            await Task.Delay(50);

            // Print text
            await _writeCharacteristic.WriteAsync(EscPosCommands.PrintLine(text));
            await Task.Delay(50);

            // Feed lines and cut
            await _writeCharacteristic.WriteAsync(EscPosCommands.FeedLines(3));
            await Task.Delay(50);

            await _writeCharacteristic.WriteAsync(EscPosCommands.FullCut());
            await Task.Delay(50);

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Print error: {ex.Message}");
            return false;
        }
    }

    public async Task DisconnectAsync()
    {
        if (_connectedDevice != null)
        {
            await _adapter.DisconnectDeviceAsync(_connectedDevice);
            _connectedDevice = null;
            _writeCharacteristic = null;
        }
    }
}
