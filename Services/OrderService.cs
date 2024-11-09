using Appwrite.Services;
using MYPM.Configurations;
using System.Text.Json;
namespace MYPM.Services;
public class OrderService
{
    private readonly Databases _databases = new AppWriteClient().GetDatabase();
    private readonly string _databaseId;
    private readonly string _ordersCollectionId;
    private List<NewOrderModel> OrdersList { get; set; } = [];

    public OrderService()
    {
        _databaseId = AppWriteClient.DatabaseId();
        _ordersCollectionId = AppWriteClient.OrderCollectionId();
    }

    public async Task<bool> CreateOrder(NewOrderModel order, ArabianOrder? arabianOrder, PanjabiOrder? panjabiOrder, SelowerOrder? selowerOrder)
    {
        try
        {
            Dictionary<string, object> data = new()
            {
                { "customerName", order.CustomerName },
                { "mobileNumber", order.MobileNumber },
                { "address", order.Address },
                { "orderDate", order.OrderDate },
                { "deliveryDate", order.DeliveryDate },
                { "paidAmount", order.PaidAmount },
                { "dueAmount", order.DueAmount },
                { "totalAmount", order.TotalAmount },
                { "status", (int)order.Status },
                { "assignTo", 111 }
            };

            if (panjabiOrder is not null)
            {
                data.Add("panjabi.amount", panjabiOrder.Amount);
                data.Add("panjabi.quantity", panjabiOrder.Quantity);
                data.Add("panjabi.length", panjabiOrder.Length);
                data.Add("panjabi.sina", panjabiOrder.Sina);
                data.Add("panjabi.komor", panjabiOrder.Komor);
                data.Add("panjabi.hata", panjabiOrder.Hata);
                data.Add("panjabi.cuff", panjabiOrder.Cuff);
                data.Add("panjabi.mohori", panjabiOrder.Mohori);
                data.Add("panjabi.rakaba", panjabiOrder.Rakaba);
                data.Add("panjabi.note", panjabiOrder.Note);
            }

            if (arabianOrder is not null)
            {
                data.Add("arabian.amount", arabianOrder.Amount);
                data.Add("arabian.quantity", arabianOrder.Quantity);
                data.Add("arabian.length", arabianOrder.Length);
                data.Add("arabian.tira", arabianOrder.Tira);
                data.Add("arabian.hata", arabianOrder.Hata);
                data.Add("arabian.cuff", arabianOrder.Cuff);
                data.Add("arabian.mohori", arabianOrder.Mohori);
                data.Add("arabian.rakaba", arabianOrder.Rakaba);
                data.Add("arabian.ness", arabianOrder.Ness);
                data.Add("arabian.note", arabianOrder.Note);
            }
            if (selowerOrder is not null)
            {
                data.Add("selower.amount", selowerOrder.Amount);
                data.Add("selower.quantity", selowerOrder.Quantity);
                data.Add("selower.length", selowerOrder.Length);
                data.Add("selower.hip", selowerOrder.Hip);
                data.Add("selower.komor", selowerOrder.Komor);
                data.Add("selower.ness", selowerOrder.Ness);
                data.Add("selower.note", selowerOrder.Note);
            }
            var orderDoc = await _databases.CreateDocument(
                _databaseId,
                _ordersCollectionId,
                order.Id,
                data
            );
            OrdersList.Add(order);
            return orderDoc is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<NewOrderModel>> GetAllOrders()
    {
        try
        {
            if (OrdersList is null || OrdersList?.Count < 1)
            {
                var response = await _databases.ListDocuments(_databaseId, _ordersCollectionId);

                if (response.Documents != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                    };

                    response.Documents.ForEach(response =>
                    {
                        string res = JsonSerializer.Serialize(response.Data);
                        var newOrder = JsonSerializer.Deserialize<NewOrderModel>(res, options);
                        if (newOrder is not null)
                        {
                            newOrder.MapEmbeddedProperties(response.Data);
                            OrdersList?.Add(newOrder);
                        }
                    });
                }
            }
            return OrdersList!;
        }
        catch (Exception ex)
        {
            _ = ex.Message.ToString();

            return await Task.FromResult(Enumerable.Empty<NewOrderModel>().ToList());
        }
    }
    public async Task<NewOrderModel> GetOrder(string Id)
    {
        try
        {
            var result = new NewOrderModel();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
            var response = await _databases.GetDocument(_databaseId, _ordersCollectionId, Id);
            if (response.Data is not null)
            {
                var obj = JsonSerializer.Serialize(response.Data, options);
                var newOrder = JsonSerializer.Deserialize<NewOrderModel>(obj, options);
                newOrder?.MapEmbeddedProperties(response.Data);
                result = newOrder!;
            }
            return result;
        }
        catch (Exception ex)
        {
            _ = ex.Message.ToString();
            return await Task.FromResult(new NewOrderModel());
        }
    }
}


