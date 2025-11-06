using CommunityToolkit.Maui;
using MYPM.Services;
using MYPM.ViewModels;
using MYPM.Pages;
using ZXing.Net.Maui.Controls;

namespace MYPM;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
         .UseMauiCommunityToolkit()
         .ConfigureFonts(fonts =>
         {
          fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
          fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemiBold");
          fonts.AddFont("fa_solid.ttf", "FontAwesome");
         })
        .UseBarcodeReader();


        builder.Services.AddTransient<IOrderService, FirestoreOrderService>();
        builder.Services.AddSingleton<IBluetoothPrinterService, BluetoothPrinterService>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<OrdersViewModel>();
        builder.Services.AddTransient<CustomerViewModel>();
        builder.Services.AddTransient<NewOrderPageViewModel>();
        builder.Services.AddTransient<EditOrderPageViewModel>();
        builder.Services.AddTransient<OrderDetailsPage>();

        return builder.Build();
    }
}