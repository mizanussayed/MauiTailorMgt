using MYPM.Pages;
using MYPM.Pages.Handheld;

namespace MYPM;
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
        Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
        Routing.RegisterRoute(nameof(OrdersPage), typeof(OrdersPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(AddCustomer), typeof(AddCustomer));
        Routing.RegisterRoute(nameof(NewOrderPage), typeof(NewOrderPage));
    }
}