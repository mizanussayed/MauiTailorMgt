using MYPM.ViewModels;

namespace MYPM.Pages;

[QueryProperty(nameof(CustomerViewModel.MobileNumber), nameof(CustomerViewModel.MobileNumber))]
public partial class CustomerOrderPage : ContentPage
{
    private readonly CustomerViewModel _viewModel;

    public CustomerOrderPage(CustomerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _viewModel.LoadCustomerOrdersCommand.ExecuteAsync(null);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not null)
        {
            var border = sender as Border;
            border!.Background = Colors.Transparent;
            _viewModel.GetDetailsCommand.Execute(e.Parameter);
        }
    }
}
