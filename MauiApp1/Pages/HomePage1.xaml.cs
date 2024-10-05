using MauiApp1.Pages.Handheld;

namespace MauiApp1.Pages;

public partial class HomePage1 : ContentPage
{
	public HomePage1()
	{
		InitializeComponent();
	}

    private async void AddNewOrderTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewOrderPage));
    }
    private async void AddNewCustomerTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddCustomer));
    }
}