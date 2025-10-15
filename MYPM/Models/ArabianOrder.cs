namespace MYPM.Models;

public sealed class ArabianOrder
{
    public int Id { get; set; }
    public int Amount { get; set; } = 500;
    public int Quantity { get; set; } = 1;
    public decimal Length { get; set; } = 50;
    public decimal Tira { get; set; } = 20;
    public decimal Hata { get; set; } = 30;
    public decimal Ber { get; set; } = 20;
    public decimal Cuff { get; set; } = 30;
    public decimal Mohori { get; set; } = 30;
    public decimal Komor { get; set; } = 35;
    public decimal Rakaba { get; set; } = 10;
    public decimal Ness { get; set; } = 30;
    public string Note { get; set; } = string.Empty;

    public int OrderId { get; set; }
}
