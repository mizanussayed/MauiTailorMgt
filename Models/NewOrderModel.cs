using System.Text.Json.Serialization;

namespace MYPM.Models;

public class NewOrderModel 
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
    public ArabianOrder? ArabianOrder { get; set; }
    public SelowerOrder? SelowerOrder { get; set; }
    public PanjabiOrder? PanjabiOrder { get; set; }

    public void MapEmbeddedProperties(Dictionary<string, object> jsonData)
    {
        if (jsonData.TryGetValue("arabian.amount", out object? value))
        {
            ArabianOrder = new ArabianOrder
            {
                Amount = Convert.ToInt32(jsonData["arabian.amount"]),
                Quantity = Convert.ToInt32(jsonData["arabian.quantity"]),
                Length = Convert.ToDecimal(jsonData["arabian.length"]),
                Tira = Convert.ToDecimal(jsonData["arabian.tira"]),
                Hata = Convert.ToDecimal(jsonData["arabian.hata"]),
                Cuff = Convert.ToDecimal(jsonData["arabian.cuff"]),
                Mohori = Convert.ToDecimal(jsonData["arabian.mohori"]),
                Rakaba = Convert.ToDecimal(jsonData["arabian.rakaba"]),
                Ness = Convert.ToDecimal(jsonData["arabian.ness"]),
                Note = jsonData["arabian.note"]?.ToString() ?? string.Empty
            };
        }

        if (jsonData.TryGetValue("panjabi.amount", out _))
        {
            PanjabiOrder = new PanjabiOrder
            {
                Amount = Convert.ToInt32(jsonData["panjabi.amount"]),
                Quantity = Convert.ToInt32(jsonData["panjabi.quantity"]),
                Length = Convert.ToDecimal(jsonData["panjabi.length"]),
                Sina = Convert.ToDecimal(jsonData["panjabi.sina"]),
                Komor = Convert.ToDecimal(jsonData["panjabi.komor"]),
                Hata = Convert.ToDecimal(jsonData["panjabi.hata"]),
                Cuff = Convert.ToDecimal(jsonData["panjabi.cuff"]),
                Mohori = Convert.ToDecimal(jsonData["panjabi.mohori"]),
                Rakaba = Convert.ToDecimal(jsonData["panjabi.rakaba"]),
                Note = jsonData["panjabi.note"]?.ToString() ?? string.Empty
            };
        }

        if (jsonData.TryGetValue("selower.amount", out _))
        {
            SelowerOrder = new SelowerOrder
            {
                Amount = Convert.ToInt32(jsonData["selower.amount"]),
                Quantity = Convert.ToInt32(jsonData["selower.quantity"]),
                Length = Convert.ToDecimal(jsonData["selower.length"]),
                Hip = Convert.ToDecimal(jsonData["selower.hip"]),
                Komor = Convert.ToDecimal(jsonData["selower.komor"]),
                Ness = Convert.ToDecimal(jsonData["selower.ness"]),
                Note = jsonData["selower.note"]?.ToString() ?? string.Empty
            };
        }
    }

}

public sealed class ArabianOrder
{
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
}

public sealed class PanjabiOrder
{
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
}

public sealed class SelowerOrder
{
    public int Amount { get; set; } = 200;
    public int Quantity { get; set; } = 1;
    public decimal Length { get; set; } = 40;
    public decimal Hip { get; set; } = 10;
    public decimal Komor { get; set; } = 35;
    public decimal Ness { get; set; } = 15;
    public string Note { get; set; } = string.Empty;
}

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Delivered
}
