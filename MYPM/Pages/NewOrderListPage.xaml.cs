using MYPM.ViewModels;

namespace MYPM.Pages;

public partial class NewOrderListPage : ContentPage
{
    private readonly OrdersViewModel _viewModel;
    public NewOrderListPage(OrdersViewModel ordersView)
    {
        InitializeComponent();
        BindingContext = _viewModel= ordersView;
        _viewModel.RefreshCommand.Execute(null);
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int id)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "OrderId", id }
            };
            Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
        }
    }
}