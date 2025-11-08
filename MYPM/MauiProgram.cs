using CommunityToolkit.Maui;
using MYPM.Services;
using MYPM.ViewModels;
using MYPM.Pages;
using MYPM.Pages.Views;
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
          fonts.AddFont("OpenSansRegular.ttf", "OpenSansRegular");
          fonts.AddFont("OpenSansSemibold.ttf", "OpenSansSemiBold");
          fonts.AddFont("fa_solid.ttf", "FontAwesome");
         })
        .UseBarcodeReader();


        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        });

        builder.Services.AddTransient<IOrderService, FirestoreOrderService>();
        builder.Services.AddSingleton<IBluetoothPrinterService, BluetoothPrinterService>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<OrdersViewModel>();
        builder.Services.AddTransient<CustomerViewModel>();
        builder.Services.AddTransient<NewOrderPageViewModel>();
        builder.Services.AddTransient<EditOrderPageViewModel>();
        builder.Services.AddTransient<OrderDetailsPage>();
        builder.Services.AddTransient<AddAdvanceAmount>();
        builder.Services.AddTransient<NewOrderPage>();

        return builder.Build();
    }
}