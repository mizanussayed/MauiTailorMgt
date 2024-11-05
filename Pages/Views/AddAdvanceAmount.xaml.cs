using MYPM.ViewModels;

namespace MYPM.Pages.Views;

public partial class AddAdvanceAmount : ContentPage
{
    public AddAdvanceAmount(NewOrderPageViewModel context)
    {
        InitializeComponent();
        BindingContext = context;
    }

    private void PaidTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is NewOrderPageViewModel context)
        {
            if (long.TryParse(PaidEntry.Text, out long paidAmount))
            {
                context.Order.PaidAmount = paidAmount;
                context.Order.DueAmount = context.Order.TotalAmount - context.Order.PaidAmount;
                DueLbl.Text = $"Due        : {(context.Order.TotalAmount - context.Order.PaidAmount).ToString()}";
            }
            else
            {
                context.Order.PaidAmount = 0;
                context.Order.DueAmount = context.Order.TotalAmount;
            }
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is NewOrderPageViewModel model)
        {
            model.SaveCommand.Execute(this);
            await Shell.Current.Navigation.PushAsync(new InvoiceQR(model.Order));
        }
    }
}