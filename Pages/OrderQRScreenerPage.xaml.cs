using MYPM.ViewModels;
using ZXing.Net.Maui;

namespace MYPM.Pages;

public partial class OrderQRScreenerPage : ContentPage
{
    private int detectCount = 0;
    public OrderQRScreenerPage()
    {
        InitializeComponent();
        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();

        if (first is not null)
        {
            Dispatcher.Dispatch(() =>
            {
                if (first.Value.Contains("Yousuf_Panjabi_tailor") && detectCount == 0)
                {
                    var IdValue = first.Value.Split("~")[1];

                    if (IdValue is not null)
                    {
                        detectCount = 1;
                        var context = new OrdersViewModel();
                        context?.GetDetailsCommand.Execute(IdValue);
                    }
                }
            });
        }
    }
}