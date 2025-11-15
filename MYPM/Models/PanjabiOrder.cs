using System.ComponentModel.DataAnnotations.Schema;

namespace MYPM.Models;

public sealed class PanjabiOrder
{
    public int Id { get; set; }
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

    [ForeignKey(nameof(NewOrder))]
    public int OrderId { get; set; }
    public NewOrderModel NewOrder { get; set; } = null!;
}
