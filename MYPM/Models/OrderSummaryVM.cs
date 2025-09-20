namespace MYPM.Models;
public sealed class OrderSummaryVM
{
    public int TotalOrders { get; set; }
    public int TotalCustomers { get; set; }
    public int MonthTotalOrders { get; set; }
    public int WeekTotalOrders { get; set; }
    public int ReadyToDelivery { get; set; }
}

