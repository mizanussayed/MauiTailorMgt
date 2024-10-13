using System.ComponentModel.DataAnnotations;

namespace MYPM.Models;

public sealed class NewOrderModel
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    public string MobileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Today;
    public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(7);
    public string OrderFor { get; set; } = "Arabian";
    public long PaidAmount { get; set; }
    public long DueAmount { get; set; }
    public long TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public sealed class ArabianOrder
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int Amount { get; set; }
    public int Quantity { get; set; }
    public decimal Length { get; set; }
    public decimal Tira { get; set; }
    public decimal Hata { get; set; }
    public decimal Cuff { get; set; }
    public decimal Mohori { get; set; }
    public decimal Rakaba { get; set; }
    public decimal Ness { get; set; }
    public string Note { get; set; } = string.Empty;
    public long AssingTo { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public sealed class PanjabiOrder
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int Amount { get; set; }
    public int Quantity { get; set; }
    public decimal Length { get; set; }
    public decimal Sina { get; set; }
    public decimal Komor { get; set; }
    public decimal Hata { get; set; }
    public decimal Cuff { get; set; }
    public decimal Mohori { get; set; }
    public decimal Rakaba { get; set; }
    public string Note { get; set; } = string.Empty;
    public long AssingTo { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public sealed class SelowerOrder
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int Amount { get; set; }
    public int Quantity { get; set; }
    public decimal Length { get; set; }
    public decimal Hip { get; set; }
    public decimal Komor { get; set; }
    public decimal Ness { get; set; }
    public string Note { get; set; } = string.Empty;
    public long AssingTo { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Delivered
}
