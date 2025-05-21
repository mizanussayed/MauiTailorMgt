namespace MYPM.Data.Models;
public sealed class OrderSummaryVM
{
    public int TotalOrders { get; set; }
    public int TotalCustomers { get; set; }
    public int MonthTotalOrders { get; set; }
    public int TodayOrders { get; set; }
    public int ReadyToDelivery { get; set; }
}

