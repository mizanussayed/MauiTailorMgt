
using ZXing.Net.Maui.Controls;

namespace MYPM.Common.QRGeneration;


public class QrResult(string url, BarcodeGeneratorView qrCode)
{
    public string Url { get; } = url;

    public BarcodeGeneratorView QrCode { get; } = qrCode;
}