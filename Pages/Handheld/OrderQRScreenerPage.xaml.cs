using MYPM.ViewModels;
using ZXing.Net.Maui;

namespace MYPM.Pages.Handheld;

public partial class OrderQRScreenerPage : ContentPage
{
    public OrderQRScreenerPage()
    {
        InitializeComponent();
        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is not null)
        {
            Dispatcher.Dispatch(() =>
            {
                if (first.Value.Contains("Yousuf_Panjabi_tailor"))
                {
                    var IdValue = first.Value.Split("~")[1];
                    if (IdValue is not null)
                    {
                        var context =new  OrdersViewModel();
                        context?.GetDetailsCommand.Execute(IdValue);
                    }
                }
            });
        }
    }
}