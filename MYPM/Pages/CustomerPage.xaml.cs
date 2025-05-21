using MYPM.ViewModels;

namespace MYPM.Pages;


[QueryProperty("SL", "SL")]
public partial class CustomerPage : ContentPage
{
	private readonly CustomerViewModel _viewModel;
    public string SL { get; set; } = string.Empty;
    public CustomerPage(CustomerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _viewModel.LoadFilteredOrdersCommand.Execute(null);
    }
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.Trim();

        if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
        {
            CustomerCollection.ItemsSource = _viewModel.Customers;
        }
        else
        {
            var filtered = _viewModel?.Customers?.ToList().FindAll(i =>
                i.CustomerName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                i.MobileNumber.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
            CustomerCollection.ItemsSource = filtered;
        }
    }
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(CustomerOrderPage)}?MobileNumber={e.Parameter}");
        }
    }
    private async void  OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(NewOrderPage)}?SL={SL}");
    }
}