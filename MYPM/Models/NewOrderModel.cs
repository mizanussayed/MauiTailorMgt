namespace MYPM.Models;

public sealed class NewOrderModel
{
    public int Id { get; set; }
    public string SL { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Today;
    public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(7);
    public string OrderFor { get; set; } = "Arabian";
    public int PaidAmount { get; set; } = 0;
    public int DueAmount { get; set; } = 0;
    public int TotalAmount { get; set; } = 0;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ICollection<PanjabiOrder> PanjabiOrders { get; set; } = [];
    public ICollection<ArabianOrder> ArabianOrders { get; set; } = [];
    public ICollection<SelowerOrder> SelowerOrders { get; set; } = [];
}
