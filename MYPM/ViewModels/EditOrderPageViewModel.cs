using MYPM.Models;
using MYPM.Services;

namespace MYPM.ViewModels;

public partial class EditOrderPageViewModel(IOrderService orderService) : ObservableObject
{
    [ObservableProperty]
    private NewOrderModel order = new();

    partial void OnOrderChanged(NewOrderModel value)
    {
        if (value is null) return;
        RecalculateTotals(value);
        OnPropertyChanged(nameof(Order));
    }

    public void RefreshComputed()
    {
        RecalculateTotals(Order);
        OnPropertyChanged(nameof(Order));
    }

    [RelayCommand]
    private void Recalc()
    {
        RefreshComputed();
    }

    private static void RecalculateTotals(NewOrderModel model)
    {
        int total = 0;
        if (model.ArabianOrders?.Count > 0)
        {
            total += model.ArabianOrders.Sum(a => a.Amount * a.Quantity);
        }
        if (model.PanjabiOrders?.Count > 0)
        {
            total += model.PanjabiOrders.Sum(p => p.Amount * p.Quantity);
        }
        if (model.SelowerOrders?.Count > 0)
        {
            total += model.SelowerOrders.Sum(s => s.Amount * s.Quantity);
        }
        model.TotalAmount = total;
        model.DueAmount = Math.Max(0, model.TotalAmount - model.PaidAmount);
    }

    [RelayCommand]
    private async Task<bool> Save()
    {
        RecalculateTotals(Order);
        var ok = await orderService.UpdateOrder(Order);
        if (ok)
            await Shell.Current.Navigation.PopAsync();
        return ok;
    }

    [RelayCommand]
    private async Task<bool> Delete()
    {
        var ok = await orderService.DeleteOrder(Order.Id);
        if (ok)
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }
        return ok;
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.Navigation.PopAsync().ConfigureAwait(false);
    }
}
