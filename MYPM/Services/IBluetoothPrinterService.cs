namespace MYPM.Services;

public interface IBluetoothPrinterService
{
    Task<List<string>> GetPairedDevicesAsync();
    Task<bool> ConnectAsync(string deviceName);
    Task<bool> PrintTextAsync(string text);
    Task<bool> PrintFormattedTextAsync(List<string> lines, int fontSize = 12, bool centerAlign = true);
    Task DisconnectAsync();
}
