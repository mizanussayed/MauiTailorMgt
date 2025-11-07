namespace MYPM.Services;

public interface IBluetoothPrinterService
{
    Task<List<string>> GetPairedDevicesAsync();
    Task<bool> ConnectAsync(string deviceName);
    Task<bool> PrintImageAsync(byte[] imageData);
    Task<bool> PrintTextAsync(string text);
    Task DisconnectAsync();
}
