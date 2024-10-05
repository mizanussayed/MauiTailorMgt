using MauiApp1.Pages;
using MauiApp1.Pages.Handheld;

namespace MauiApp1;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        InitRoutes();
    }

    private void InitRoutes()
    {
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(HomePage1), typeof(HomePage1));
        Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
        Routing.RegisterRoute(nameof(OrdersPage), typeof(OrdersPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(AddCustomer), typeof(AddCustomer));
        Routing.RegisterRoute(nameof(NewOrderPage), typeof(NewOrderPage));
    }
}