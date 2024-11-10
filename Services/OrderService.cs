using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
using MYPM.Configurations;
using System.Text.Json;
namespace MYPM.Services;
public class OrderService
{
    private readonly Databases _databases = new AppWriteClient().GetDatabase();
    private readonly string _databaseId;
    private readonly string _ordersCollectionId;
    private readonly Storage _storage = new AppWriteClient().GetStorage();
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
                data.Add("panjabiAmount", panjabiOrder.Amount);
                data.Add("panjabiQuantity", panjabiOrder.Quantity);
                data.Add("panjabiLength", panjabiOrder.Length);
                data.Add("panjabiSina", panjabiOrder.Sina);
                data.Add("panjabiKomor", panjabiOrder.Komor);
                data.Add("panjabiHata", panjabiOrder.Hata);
                data.Add("panjabiCuff", panjabiOrder.Cuff);
                data.Add("panjabiMohori", panjabiOrder.Mohori);
                data.Add("panjabiRakaba", panjabiOrder.Rakaba);
                data.Add("panjabiNote", panjabiOrder.Note);
            }

            if (arabianOrder is not null)
            {
                data.Add("arabianAmount", arabianOrder.Amount);
                data.Add("arabianQuantity", arabianOrder.Quantity);
                data.Add("arabianLength", arabianOrder.Length);
                data.Add("arabianTira", arabianOrder.Tira);
                data.Add("arabianHata", arabianOrder.Hata);
                data.Add("arabianCuff", arabianOrder.Cuff);
                data.Add("arabianMohori", arabianOrder.Mohori);
                data.Add("arabianRakaba", arabianOrder.Rakaba);
                data.Add("arabianNess", arabianOrder.Ness);
                data.Add("arabianNote", arabianOrder.Note);
            }
            if (selowerOrder is not null)
            {
                data.Add("selowerAmount", selowerOrder.Amount);
                data.Add("selowerQuantity", selowerOrder.Quantity);
                data.Add("selowerLength", selowerOrder.Length);
                data.Add("selowerHip", selowerOrder.Hip);
                data.Add("selowerKomor", selowerOrder.Komor);
                data.Add("selowerNess", selowerOrder.Ness);
                data.Add("selowerNote", selowerOrder.Note);
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
                //var query = Query.Select(["$id", "customerName", "mobileNumber", "orderDate", "deliveryDate", "paidAmount", "dueAmount", "totalAmount", "status"]);
                //var response = await _databases.ListDocuments(_databaseId, _ordersCollectionId, [query]);
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

    public async Task<NewOrderModel> UpdateStatus(string Id, OrderStatus status)
    {
        try
        {
            Dictionary<string, object> data = new()
            {
                { "status", (int)status },
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
            var response = await _databases.UpdateDocument(
                  _databaseId,
                  _ordersCollectionId,
                  Id,
                  data
             );
            if (response.Data is not null)
            {
                var obj = JsonSerializer.Serialize(response.Data, options);
                var newOrder = JsonSerializer.Deserialize<NewOrderModel>(obj, options);
                newOrder?.MapEmbeddedProperties(response.Data);
                return newOrder!;
            }
            return await Task.FromResult(new NewOrderModel());
        }
        catch (Exception)
        {
            return await Task.FromResult(new NewOrderModel());
        }
    }

    public async Task<bool> UplaodGelleryImages(InputFile fileUrl)
    {
        try
        {
            var response = await _storage.CreateFile("673104ef0029440a9c01", ID.Unique(), fileUrl);
            return response is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}


