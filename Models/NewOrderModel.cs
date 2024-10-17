using MYPM.Pages.Handheld;
using System.Text.Json.Serialization;

namespace MYPM.Models;

public partial class NewOrderModel : ObservableObject
{
    [JsonPropertyName("$id")]
    public string Id { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Today;
    public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(7);
    public string OrderFor { get; set; } = "Arabian";
    public long PaidAmount { get; set; } = 0;
    public long DueAmount { get; set; } = 0;
    public long TotalAmount { get; set; } = 0;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;


    private static readonly Random _random = new Random();

    private static readonly string[] brushes = ["#FFB572", "#65B0F6", "#FF7CA3", "#50D1AA", "#9290FE"];
    public static Brush RandomBrush
    {
        get
        {
            var id = _random.Next(0, 4);
            return new SolidColorBrush(Color.Parse(brushes[id]));
        }
    }

    [RelayCommand]
    private async Task Pay()
    {
        try
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Order", this }
            };
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}", navigationParameter);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}

public sealed class ArabianOrder
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int Amount { get; set; } = 500;
    public int Quantity { get; set; } = 1;
    public decimal Length { get; set; } = 50;
    public decimal Tira { get; set; } = 20;
    public decimal Hata { get; set; } = 30;
    public decimal Cuff { get; set; } = 30;
    public decimal Mohori { get; set; } = 30;
    public decimal Rakaba { get; set; } = 10;
    public decimal Ness { get; set; } = 30;
    public string Note { get; set; } = string.Empty;
    public long AssignTo { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public sealed class PanjabiOrder
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int Amount { get; set; } = 300;
    public int Quantity { get; set; } = 1;
    public decimal Length { get; set; } = 30;
    public decimal Sina { get; set; } = 20;
    public decimal Komor { get; set; } = 30;
    public decimal Hata { get; set; } = 10;
    public decimal Cuff { get; set; } = 30;
    public decimal Mohori { get; set; } = 22;
    public decimal Rakaba { get; set; } = 30;
    public string Note { get; set; } = string.Empty;
    public long AssignTo { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public sealed class SelowerOrder
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int Amount { get; set; } = 200;
    public int Quantity { get; set; } = 1;
    public decimal Length { get; set; } = 40;
    public decimal Hip { get; set; } = 10;
    public decimal Komor { get; set; } = 35;
    public decimal Ness { get; set; } = 15;
    public string Note { get; set; } = string.Empty;
    public long AssignTo { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Delivered
}
