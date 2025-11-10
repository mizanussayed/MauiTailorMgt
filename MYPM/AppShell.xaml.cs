using MYPM.Pages;
using MYPM.Pages.Views;

#if ANDROID
using MYPM.Platforms.Android;
#endif

namespace MYPM;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        InitRoutes();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetupStatusBar();
    }

    private void SetupStatusBar()
    {
        try
        {
            if (Application.Current?.MainPage?.Window is Window window &&
                Application.Current?.Resources.TryGetValue("SurfaceVariant", out var surfaceVariant) == true &&
                    surfaceVariant is Color statusBarColor)
            {
                window.SetStatusBarColor(statusBarColor, darkContent: true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting status bar: {ex.Message}");
        }
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