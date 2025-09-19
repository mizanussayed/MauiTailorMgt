using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MYPM.Data.Configurations;
using MYPM.Services;
using MYPM.ViewModels;
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
        var connectionString = "";
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, sqlOptions =>
            {
               sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Public")
                         .EnableRetryOnFailure();
            });
        });

        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<OrdersViewModel>();
        builder.Services.AddTransient<CustomerViewModel>();
        builder.Services.AddTransient<NewOrderPageViewModel>();
        builder.Services.AddTransient<EditOrderPageViewModel>();

        return builder.Build();
    }
}