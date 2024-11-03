using MYPM.Pages;
using MYPM.Pages.Handheld;
using MYPM.Pages.Views;

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
        Routing.RegisterRoute(nameof(CustomerListPage), typeof(CustomerListPage));
        Routing.RegisterRoute(nameof(NewOrderPage), typeof(NewOrderPage));
        Routing.RegisterRoute(nameof(InvoiceListPage), typeof(InvoiceListPage));
        Routing.RegisterRoute(nameof(InvoiceQR), typeof(InvoiceQR));
        Routing.RegisterRoute(nameof(NewOrderListPage), typeof(NewOrderListPage));
    }
}