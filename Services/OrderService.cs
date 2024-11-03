using Appwrite.Services;
using Appwrite;
using Appwrite.Models;
using MYPM.Configurations;
using System.Text.Json;
namespace MYPM.Services;
public class OrderService
{
    private readonly Databases _databases = new AppWriteClient().GetDatabase();
    private readonly string _databaseId;
    private readonly string _ordersCollectionId;
    private readonly string _arabianOrderCollectionId;
    private readonly string _panjabiOrderCollectionId;
    private readonly string _selowerOrderCollectionId;

    public OrderService()
    {
        _databaseId = AppWriteClient.DatabaseId();
        _ordersCollectionId = AppWriteClient.OrderCollectionId();
        _arabianOrderCollectionId = AppWriteClient.ArabianCollectionId();
        _panjabiOrderCollectionId = AppWriteClient.PanjabiCollectionId();
        _selowerOrderCollectionId = AppWriteClient.SelowerCollectionId();
    }

    public async Task<bool> CreateOrder(NewOrderModel order, ArabianOrder? arabianOrder, PanjabiOrder? panjabiOrder, SelowerOrder? selowerOrder)
    {
        try
        {
            var orderDoc = await _databases.CreateDocument(
                _databaseId,
                _ordersCollectionId,
                order.Id,
                new Dictionary<string, object>
                {
                { "customerName", order.CustomerName },
                { "mobileNumber", order.MobileNumber },
                { "address", order.Address },
                { "orderDate", order.OrderDate },
                { "deliveryDate", order.DeliveryDate },
                { "orderFor", order.OrderFor },
                { "paidAmount", order.PaidAmount },
                { "dueAmount", order.DueAmount },
                { "totalAmount", order.TotalAmount },
                { "status", (int)order.Status }
                }
            );


            if (arabianOrder != null)
            {
                await _databases.CreateDocument(
                    _databaseId,
                    _arabianOrderCollectionId,
                    ID.Unique(),
                    new Dictionary<string, object>
                    {
                    { "orderId", orderDoc.Id },
                    { "amount", arabianOrder.Amount },
                    { "quantity", arabianOrder.Quantity },
                    { "length", arabianOrder.Length },
                    { "tira", arabianOrder.Tira },
                    { "hata", arabianOrder.Hata },
                    { "cuff", arabianOrder.Cuff },
                    { "mohori", arabianOrder.Mohori },
                    { "rakaba", arabianOrder.Rakaba },
                    { "ness", arabianOrder.Ness },
                    { "note", arabianOrder.Note },
                    { "assignTo", arabianOrder.AssignTo },
                    { "status", (int)arabianOrder.Status }
                    }
                );
            }

            if (panjabiOrder != null)
            {
                await _databases.CreateDocument(
                   _databaseId,
                   _panjabiOrderCollectionId,
                   ID.Unique(),
                   new Dictionary<string, object>
                   {
                    { "orderId", orderDoc.Id },
                    { "amount", panjabiOrder.Amount },
                    { "quantity", panjabiOrder.Quantity },
                    { "length", panjabiOrder.Length },
                    { "sina", panjabiOrder.Sina },
                    { "komor", panjabiOrder.Komor },
                    { "hata", panjabiOrder.Hata },
                    { "cuff", panjabiOrder.Cuff },
                    { "mohori", panjabiOrder.Mohori },
                    { "rakaba", panjabiOrder.Rakaba },
                    { "note", panjabiOrder.Note },
                    { "assignTo", panjabiOrder.AssignTo },
                    { "status", (int)panjabiOrder.Status }
                   }
               );
            }

            if (selowerOrder != null)
            {
                await _databases.CreateDocument(
                    _databaseId,
                    _selowerOrderCollectionId,
                    ID.Unique(),
                    new Dictionary<string, object>
                    {
                    { "orderId", orderDoc.Id },
                    { "amount", selowerOrder.Amount },
                    { "quantity", selowerOrder.Quantity },
                    { "length", selowerOrder.Length },
                    { "hip", selowerOrder.Hip },
                    { "komor", selowerOrder.Komor },
                    { "ness", selowerOrder.Ness },
                    { "note", selowerOrder.Note },
                    { "assignTo", selowerOrder.AssignTo },
                    { "status", (int)selowerOrder.Status }
                    }
                );
            }
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
            var result = new List<NewOrderModel>();
            var response = await _databases.ListDocuments(_databaseId, _ordersCollectionId);

            if (response.Documents != null)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                };

                result = response.Documents.Select(document =>
                {
                    string res = JsonSerializer.Serialize(document.Data, options);
                    return JsonSerializer.Deserialize<NewOrderModel>(res, options);
                }).OrderByDescending(d => d.OrderDate).ToList();
            }
            return result;
        }
        catch (Exception ex)
        {
            _ = ex.Message.ToString();

            return await Task.FromResult(Enumerable.Empty<NewOrderModel>().ToList());
        }
    }
    public async  Task<NewOrderModel> GetOrder(string Id)
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
            if (response.Id  != null)
            {
                var obj = JsonSerializer.Serialize(response.Data, options);
                result = JsonSerializer.Deserialize<NewOrderModel>(obj, options);
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


