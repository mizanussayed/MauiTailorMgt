using MYPM.Pages;
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
        Routing.RegisterRoute(nameof(OrderQRScreenerPage), typeof(OrderQRScreenerPage));
        Routing.RegisterRoute(nameof(GalleryListPage), typeof(GalleryListPage));
        Routing.RegisterRoute(nameof(CustomerPage), typeof(CustomerPage));
        Routing.RegisterRoute(nameof(CustomerOrderPage), typeof(CustomerOrderPage));
        Routing.RegisterRoute(nameof(NewOrderPage), typeof(NewOrderPage));
        Routing.RegisterRoute(nameof(InvoiceQR), typeof(InvoiceQR));
        Routing.RegisterRoute(nameof(NewOrderListPage), typeof(NewOrderListPage));
        Routing.RegisterRoute(nameof(EditOrderPage), typeof(EditOrderPage));
    }
}