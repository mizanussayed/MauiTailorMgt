using MYPM.ViewModels;

namespace MYPM.Pages;

public partial class NewOrderListPage : ContentPage
{
	public NewOrderListPage()
	{
		InitializeComponent();
        BindingContext = new OrdersViewModel();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not null)
        {
            var border = sender as Border;
            border!.Background = Colors.Transparent;
            var context = BindingContext as OrdersViewModel;
            context?.GetDetailsCommand.Execute(e.Parameter);
        }
    }
}