using Microsoft.Maui.Controls.Shapes;
using MYPM.Common.QRGeneration;
using MYPM.Data.Models;
using Path = System.IO.Path;

namespace MYPM.Pages.Views;
public partial class InvoiceQR : ContentPage
{
    private string _orderModelFilePath = string.Empty;
    private readonly NewOrderModel? orderModel;
    public InvoiceQR(NewOrderModel order)
    {
        InitializeComponent();
        orderModel = order;
        GenerateInvoice().ConfigureAwait(true);
    }


    private async Task GenerateInvoice()
    {
        Label header = new() { Text = "Yousuf Tailors", FontSize = 24, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label info = new() { Text = orderModel!.CustomerName, FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };
        Label infoPhone = new() { Text = orderModel.MobileNumber, FontSize = 14, HorizontalOptions = LayoutOptions.Center, TextColor = Colors.Black };

        var barcode = QrUtils.MakeQrCodeResult(CreateQRText(orderModel)).QrCode;
        barcode.WidthRequest = 200;
        barcode.HeightRequest = 200;
        barcode.HorizontalOptions = LayoutOptions.End;

        var border = new Border
        {
            Stroke = Colors.Orange,
            Padding = new Thickness(2),
            Margin = new Thickness(0, 1),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            BackgroundColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout { header, info, infoPhone, barcode }
        };

        qrBox.Add(border);
        _orderModelFilePath = await SaveInvoiceAsImage(border).ConfigureAwait(false);
    }

    private static async Task<string> SaveInvoiceAsImage(VisualElement visualElement)
    {
        await Task.Delay(100);
        string fileName = Path.Combine(FileSystem.CacheDirectory, "orderModel.png");
        try
        {
            var image = await visualElement.CaptureAsync();
            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            if (image is not null)
                await image.CopyToAsync(stream, ScreenshotFormat.Png);
            return await Task.FromResult(fileName);
        }
        catch (Exception)
        {
            return await Task.FromResult(string.Empty);
        }
    }


    private async void OnShareInvoiceClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_orderModelFilePath))
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Invoice",
                File = new ShareFile(_orderModelFilePath)
            }).ConfigureAwait(false);
        }
        else
        {
            await DisplayActionSheet("Error", "OK", null, "No orderModel to share.").ConfigureAwait(false);
        }
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }

    private static Task<Label> CreateLabel(string text)
    {
        return Task.FromResult(new Label
        {
            Text = text,
            TextColor = Colors.Black,
            FontSize = 14,
        });
    }

    private static string CreateQRText(NewOrderModel orderModel)
    {
        return $"Yousuf_Panjabi_tailor~{orderModel.Id}~Customer: {orderModel.CustomerName}Mobile: {orderModel.MobileNumber}";
    }
}
