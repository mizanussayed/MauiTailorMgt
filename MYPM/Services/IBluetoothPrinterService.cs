namespace MYPM.Services;

public interface IBluetoothPrinterService
{
    List<string> GetPairedDevices();
    Task<bool> ConnectAsync(string deviceName);
    Task<bool> PrintImageAsync(byte[] imageData);
    Task<bool> PrintTextAsync(string text);
    Task DisconnectAsync();
}
