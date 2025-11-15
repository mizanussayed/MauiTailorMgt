using System.ComponentModel.DataAnnotations.Schema;

namespace MYPM.Models;

public sealed class SelowerOrder
{
    public int Id { get; set; }
    public int Amount { get; set; } = 200;
    public int Quantity { get; set; } = 1;
    public decimal Length { get; set; } = 40;
    public decimal Hip { get; set; } = 10;
    public decimal Komor { get; set; } = 35;
    public decimal Ness { get; set; } = 15;
    public string Note { get; set; } = string.Empty;

    [ForeignKey(nameof(NewOrder))]
    public int OrderId { get; set; }
    public NewOrderModel NewOrder { get; set; } = null!;
}
