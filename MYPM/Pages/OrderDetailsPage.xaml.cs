using MYPM.Common;
using MYPM.Models;
using MYPM.Pages.Views;
using MYPM.Services;
using MYPM.ViewModels;

namespace MYPM.Pages;

[QueryProperty(nameof(OrderId), "OrderId")]
public partial class OrderDetailsPage : ContentPage
{
    private int NewId;
    private NewOrderModel? OrderModel;
    private readonly OrdersViewModel _orderViewModel;
    private readonly IOrderService _orderService;
    private readonly IBluetoothPrinterService _printerService;
    private readonly EditOrderPageViewModel _editOrderViewModel;

    public int OrderId
    {
        get => NewId;
        set
        {
            NewId = value;
            _ = LoadData();
        }
    }

    public OrderDetailsPage(
        OrdersViewModel viewModel,
        IOrderService orderService,
        IBluetoothPrinterService printerService,
        EditOrderPageViewModel editOrderViewModel)
    {
        InitializeComponent();
        _orderViewModel = viewModel;
        _orderService = orderService;
        _printerService = printerService;
        _editOrderViewModel = editOrderViewModel;
    }

    private async Task LoadData()
    {
        OrderModel = await _orderViewModel.LoadOrderById(NewId);
        BindingContext = OrderModel;

        if (OrderModel?.ArabianOrders?.Count > 0)
            ArabianOrderLayout.IsVisible = true;

        if (OrderModel?.PanjabiOrders?.Count > 0)
            PanjabiOrderLayout.IsVisible = true;

        if (OrderModel?.SelowerOrders?.Count > 0)
            SelowerOrderLayout.IsVisible = true;

    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        if (OrderModel == null)
        {
            await DisplayAlert("Error", "Order details are not available to share.", "OK");
            return;
        }
        await Navigation.PushModalAsync(new ShareQR(OrderModel));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (OrderModel is null) return;
        var nav = new Dictionary<string, object> { ["Order"] = OrderModel };
        await Shell.Current.GoToAsync(nameof(EditOrderPage), nav);
    }

    private async void OnQuickAddClicked(object sender, EventArgs e)
    {
        if (OrderModel is null) return;

        try
        {
            var summary = await _orderService.GetOrderSummary();
            var newSerialNumber = GenerateOrderSerial.GetSL(summary.WeekTotalOrders + 1);

            var newOrder = new NewOrderModel
            {
                Id = 0, // New order will get new ID
                SL = newSerialNumber, // Set the new serial number
                CustomerName = OrderModel.CustomerName,
                MobileNumber = OrderModel.MobileNumber,
                Address = OrderModel.Address,
                OrderDate = DateTime.Today,
                DeliveryDate = DateTime.Today.AddDays(7),
                OrderFor = OrderModel.OrderFor,
                PaidAmount = 0,
                DueAmount = 0,
                TotalAmount = 0,
                Status = OrderStatus.Pending
            };

            if (OrderModel.ArabianOrders?.Count > 0)
            {
                newOrder.ArabianOrders = [.. OrderModel.ArabianOrders.Select(ao => new ArabianOrder
                    {
                        Id = 0,
                        Amount = ao.Amount,
                        Quantity = ao.Quantity,
                        Length = ao.Length,
                        Tira = ao.Tira,
                        Hata = ao.Hata,
                        Ber = ao.Ber,
                        Cuff = ao.Cuff,
                        Mohori = ao.Mohori,
                        Komor = ao.Komor,
                        Rakaba = ao.Rakaba,
                        Ness = ao.Ness
                    })];
            }

            if (OrderModel.PanjabiOrders?.Count > 0)
            {
                newOrder.PanjabiOrders = [.. OrderModel.PanjabiOrders.Select(po => new PanjabiOrder
                    {
                        Id = 0,
                        Amount = po.Amount,
                        Quantity = po.Quantity,
                        Length = po.Length,
                        Sina = po.Sina,
                        Komor = po.Komor,
                        Hata = po.Hata,
                        Cuff = po.Cuff,
                        Mohori = po.Mohori,
                        Rakaba = po.Rakaba
                    })];
            }

            if (OrderModel.SelowerOrders?.Count > 0)
            {
                newOrder.SelowerOrders = [.. OrderModel.SelowerOrders.Select(so => new SelowerOrder
                    {
                        Id = 0,
                        Amount = so.Amount,
                        Quantity = so.Quantity,
                        Length = so.Length,
                        Hip = so.Hip,
                        Komor = so.Komor,
                        Ness = so.Ness
                    })];
            }

            // Calculate total amount
            int totalAmount = 0;
            foreach (var ao in newOrder.ArabianOrders)
                totalAmount += ao.Amount * ao.Quantity;
            foreach (var po in newOrder.PanjabiOrders)
                totalAmount += po.Amount * po.Quantity;
            foreach (var so in newOrder.SelowerOrders)
                totalAmount += so.Amount * so.Quantity;

            newOrder.TotalAmount = totalAmount;
            newOrder.DueAmount = totalAmount;

            _editOrderViewModel.Order = newOrder;

            await Shell.Current.Navigation.PushModalAsync(
                new AddAdvanceAmount(_editOrderViewModel, _printerService),
                true);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to create quick add order: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (OrderModel is null) return;
        var confirm = await DisplayAlert("Delete", $"Delete order {OrderModel.SL}?", "Yes", "No");
        if (!confirm) return;
        var isDeleted = await _orderViewModel.Delete(OrderModel.Id);

        if (isDeleted)
        {
            await _orderViewModel.RefreshCommand.ExecuteAsync(null);
            await Shell.Current.Navigation.PopToRootAsync();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to delete order.", "OK");
        }
    }
}
