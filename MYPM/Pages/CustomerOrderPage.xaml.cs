using MYPM.ViewModels;

namespace MYPM.Pages;

[QueryProperty(nameof(CustomerViewModel.MobileNumber), nameof(CustomerViewModel.MobileNumber))]
public partial class CustomerOrderPage : ContentPage
{
    private readonly CustomerViewModel _viewModel;

    public string MobileNumber
    {
        get => _viewModel?.MobileNumber ?? string.Empty;
        set
        {
            if (_viewModel is not null)
            {
                _viewModel.MobileNumber = value;
            }
        }
    }

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
            _viewModel.GetDetailsCommand.Execute(e.Parameter);
        }
    }
}
