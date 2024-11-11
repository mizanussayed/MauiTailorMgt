using MYPM.Pages.Handheld;
using MYPM.ViewModels;

namespace MYPM.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }

    private async void AddNewOrderTapped(object sender, TappedEventArgs e)
    {
        var total = BindingContext as HomeViewModel;
        await Shell.Current.GoToAsync($"{nameof(NewOrderPage)}?NextId={total?.NextId}");
    }
    private async void AddNewCustomerTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewOrderListPage));
    }  
    
    private async void ViewCustomerListTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewOrderListPage));
    }

    private async void AssignOrderTapped(object sender, TappedEventArgs e)
    {

        await Shell.Current.GoToAsync(nameof(NewOrderListPage));
    }

    private async void ViewInvoicesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewOrderListPage));
    } 
    private async void ViewGalleryTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(GalleryListPage));
    }
}