
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace MYPM.Common.QRGeneration;
public static class QrUtils
{
    public static VerticalStackLayout MakeQrCode(string text)
    {
        var barcode = new BarcodeGeneratorView
        {
            Format = BarcodeFormat.QrCode,
            Value = text,
        };
        var result = new VerticalStackLayout
        {
            WidthRequest = 200,
            HeightRequest = 200,
            VerticalOptions = LayoutOptions.Center,
        };
        result.Add(barcode);
        return result;
    } 
    
    public static QrResult MakeQrCodeResult(string text)
    {
        var barcode = new BarcodeGeneratorView
        {
            Format = BarcodeFormat.QrCode,
            Value = text,
        };
        return new QrResult(text, barcode);
    }
}
