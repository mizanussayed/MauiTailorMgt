using MYPM.ViewModels;

namespace MYPM.Pages;

public partial class NewOrderListPage : ContentPage
{
    private readonly OrdersViewModel _viewModel;
    public NewOrderListPage(OrdersViewModel ordersView)
    {
        InitializeComponent();
        BindingContext = _viewModel= ordersView ;
        _viewModel.RefreshCommand.Execute(null);
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