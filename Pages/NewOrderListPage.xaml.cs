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
	
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string v = (e.CurrentSelection.FirstOrDefault() as NewOrderModel).Id;
        var context = BindingContext as OrdersViewModel;
        context?.GetDetailsCommand.Execute(v);    
    }
}