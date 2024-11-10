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
        if (jsonData.TryGetValue("arabianAmount", out _))
        {
            ArabianOrder = new ArabianOrder
            {
                Amount = Convert.ToInt32(jsonData["arabianAmount"]),
                Quantity = Convert.ToInt32(jsonData["arabianQuantity"]),
                Length = Convert.ToDecimal(jsonData["arabianLength"]),
                Tira = Convert.ToDecimal(jsonData["arabianTira"]),
                Hata = Convert.ToDecimal(jsonData["arabianHata"]),
                Cuff = Convert.ToDecimal(jsonData["arabianCuff"]),
                Mohori = Convert.ToDecimal(jsonData["arabianMohori"]),
                Rakaba = Convert.ToDecimal(jsonData["arabianRakaba"]),
                Ness = Convert.ToDecimal(jsonData["arabianNess"]),
                Note = jsonData["arabianNote"]?.ToString() ?? string.Empty
            };
        }

        if (jsonData.TryGetValue("panjabiAmount", out _))
        {
            PanjabiOrder = new PanjabiOrder
            {
                Amount = Convert.ToInt32(jsonData["panjabiAmount"]),
                Quantity = Convert.ToInt32(jsonData["panjabiQuantity"]),
                Length = Convert.ToDecimal(jsonData["panjabiLength"]),
                Sina = Convert.ToDecimal(jsonData["panjabiSina"]),
                Komor = Convert.ToDecimal(jsonData["panjabiKomor"]),
                Hata = Convert.ToDecimal(jsonData["panjabiHata"]),
                Cuff = Convert.ToDecimal(jsonData["panjabiCuff"]),
                Mohori = Convert.ToDecimal(jsonData["panjabiMohori"]),
                Rakaba = Convert.ToDecimal(jsonData["panjabiRakaba"]),
                Note = jsonData["panjabiNote"]?.ToString() ?? string.Empty
            };
        }

        if (jsonData.TryGetValue("selowerAmount", out _))
        {
            SelowerOrder = new SelowerOrder
            {
                Amount = Convert.ToInt32(jsonData["selowerAmount"]),
                Quantity = Convert.ToInt32(jsonData["selowerQuantity"]),
                Length = Convert.ToDecimal(jsonData["selowerLength"]),
                Hip = Convert.ToDecimal(jsonData["selowerHip"]),
                Komor = Convert.ToDecimal(jsonData["selowerKomor"]),
                Ness = Convert.ToDecimal(jsonData["selowerNess"]),
                Note = jsonData["selowerNote"]?.ToString() ?? string.Empty
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
