using MYPM.ViewModels;

namespace MYPM.Pages;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.RefreshDataCommand.Execute(null);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _viewModel.RefreshDataCommand.Execute(null);
    }

    private async void AddNewOrderTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(NewOrderPage)}?SL={_viewModel?.SL}");
    }
    private async void ViewCustomerListTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(CustomerPage)}?SL={_viewModel?.SL}");
    }

    private async void ViewGalleryTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(GalleryListPage));
    }
}