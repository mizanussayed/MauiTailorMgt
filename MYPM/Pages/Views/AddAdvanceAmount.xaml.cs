using CommunityToolkit.Maui.Views;
using MYPM.ViewModels;

namespace MYPM.Pages.Views;

public partial class AddAdvanceAmount : ContentPage
{
    private readonly NewOrderPageViewModel context;
    public AddAdvanceAmount(NewOrderPageViewModel ctx)
    {
        InitializeComponent();
        BindingContext = ctx;
        context = ctx;
    }

    private void PaidTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (int.TryParse(PaidEntry?.Text, out int paidAmount))
            {
                context.Order.PaidAmount = paidAmount;
                context.Order.DueAmount = context.Order.TotalAmount - context.Order.PaidAmount;
                DueLbl.Text = $"Due          : {(context.Order.TotalAmount - context.Order.PaidAmount).ToString()}";
            }
            else
            {
                context.Order.PaidAmount = 0;
                context.Order.DueAmount = context.Order.TotalAmount;
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        context.SaveCommand.Execute(this);
        await Shell.Current.Navigation.PushModalAsync(new InvoiceQR(context.Order));
    }
}