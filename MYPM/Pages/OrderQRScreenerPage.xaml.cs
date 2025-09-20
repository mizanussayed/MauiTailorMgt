using MYPM.ViewModels;
using ZXing.Net.Maui;
namespace MYPM.Pages;

public partial class OrderQRScreenerPage : ContentPage
{
    private int detectCount = 0;
    private readonly OrdersViewModel viewModel;
    private bool torchOn;

    public OrderQRScreenerPage(OrdersViewModel _viewModel)
    {
        InitializeComponent();

        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All, 
            AutoRotate = true,
            Multiple = false
        };

        viewModel = _viewModel;
        StartScanLineAnimation();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        barcodeView.IsDetecting = true;
        barcodeView.IsEnabled = true;
        detectCount = 0;
        StatusLabel.Text = "Point the camera at a QR code";
    }

    protected override void OnDisappearing()
    {
        barcodeView.IsDetecting = false;
        barcodeView.IsEnabled = false;
        base.OnDisappearing();
    }

    private void StartScanLineAnimation()
    {
        const uint duration = 1000;
        async void animate()
        {
            while (true)
            {
                try
                {
                    await scanLine.TranslateTo(0, Viewfinder.Height, duration, Easing.Linear);
                    await scanLine.TranslateTo(0, 0, duration, Easing.Linear);
                }
                catch
                {
                    break;
                }
            }
        }
        animate();
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is null) return;

        Dispatcher.Dispatch(() =>
        {
            if (detectCount != 0) return;
            if (!first.Value.Contains("Yousuf_Panjabi_tailor")) return;

            var IdValue = first.Value.Split("~").ElementAtOrDefault(1);
            if (string.IsNullOrWhiteSpace(IdValue)) return;

            detectCount = 1;
            barcodeView.IsDetecting = false; 
            StatusLabel.Text = "Processing...";

            try
            {
                viewModel?.GetDetailsCommand.Execute(int.Parse(IdValue));
                StatusLabel.Text = "Order found ✔️";
                try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }
            }
            catch
            {
                StatusLabel.Text = "Failed to load order ❌";
                try { HapticFeedback.Default.Perform(HapticFeedbackType.LongPress); } catch { }
                barcodeView.IsDetecting = true; 
                detectCount = 0;
            }
        });
    }

    private void ToggleTorchClicked(object sender, EventArgs e)
    {
        torchOn = !torchOn;
        barcodeView.IsTorchOn = torchOn;
    }
}
