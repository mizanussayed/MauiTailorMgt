using Google.Cloud.Firestore;
using MYPM.Common;
using MYPM.Models;
using MYPM.Services.Converters;

namespace MYPM.Services;

public sealed class FirestoreOrderService : IOrderService
{
    private const string OrdersCollection = "Orders";
    private const string CountersDoc = "Meta/Counters";

    private static readonly Lazy<Task<FirestoreDb>> _lazyDb = new(() => CreateDb());
    private static readonly DecimalAsScaledLongConverter s_decimalConv = new();
    private static async Task<FirestoreDb> CreateDb()
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync("adminsdk.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var db = new FirestoreDbBuilder
            {
                JsonCredentials = json,
                ProjectId = "mypm-tailor-ffd0f",
           
            }.Build();

            return db;
        }
        catch
        {
            return null!;
        }
    }

    private static Task<FirestoreDb> GetDbAsync() => _lazyDb.Value;

    private static (DateTime UtcStart, DateTime UtcEnd) GetCurrentWeekUtcRange()
    {
        var utcNow = DateTime.UtcNow;
        var today = utcNow.Date;
        int daysSinceSunday = (int)today.DayOfWeek;
        var startOfWeek = today.AddDays(-daysSinceSunday - 1); // keep -1 to mimic previous EF logic
        var endOfWeek = startOfWeek.AddDays(7); // exclusive
        return (startOfWeek, endOfWeek);
    }

    private static Timestamp ToTs(DateTime dt)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dt, DateTimeKind.Utc));

    private static DateTime FromTs(object? value)
    {
        if (value is Timestamp ts)
            return ts.ToDateTime().ToLocalTime();
        if (value is DateTime dt)
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc).ToLocalTime();
        return DateTime.Now;
    }

    private static Dictionary<string, object> ToDoc(NewOrderModel o)
    {
        return new Dictionary<string, object>
        {
            [nameof(NewOrderModel.Id)] = o.Id,
            [nameof(NewOrderModel.SL)] = o.SL,
            [nameof(NewOrderModel.CustomerName)] = o.CustomerName,
            [nameof(NewOrderModel.MobileNumber)] = o.MobileNumber,
            [nameof(NewOrderModel.Address)] = o.Address,
            [nameof(NewOrderModel.OrderDate)] = ToTs(o.OrderDate.ToUniversalTime()),
            [nameof(NewOrderModel.DeliveryDate)] = ToTs(o.DeliveryDate.ToUniversalTime()),
            [nameof(NewOrderModel.OrderFor)] = o.OrderFor,
            [nameof(NewOrderModel.PaidAmount)] = o.PaidAmount,
            [nameof(NewOrderModel.DueAmount)] = o.DueAmount,
            [nameof(NewOrderModel.TotalAmount)] = o.TotalAmount,
            [nameof(NewOrderModel.Status)] = o.Status.ToString(),
            [nameof(NewOrderModel.PanjabiOrders)] = o.PanjabiOrders.Select(p => new Dictionary<string, object>
            {
                [nameof(PanjabiOrder.Id)] = p.Id,
                [nameof(PanjabiOrder.Amount)] = p.Amount,
                [nameof(PanjabiOrder.Quantity)] = p.Quantity,
                [nameof(PanjabiOrder.Length)] = s_decimalConv.ToFirestore(p.Length),
                [nameof(PanjabiOrder.Sina)] = s_decimalConv.ToFirestore(p.Sina),
                [nameof(PanjabiOrder.Komor)] = s_decimalConv.ToFirestore(p.Komor),
                [nameof(PanjabiOrder.Hata)] = s_decimalConv.ToFirestore(p.Hata),
                [nameof(PanjabiOrder.Cuff)] = s_decimalConv.ToFirestore(p.Cuff),
                [nameof(PanjabiOrder.Mohori)] = s_decimalConv.ToFirestore(p.Mohori),
                [nameof(PanjabiOrder.Rakaba)] = s_decimalConv.ToFirestore(p.Rakaba),
                [nameof(PanjabiOrder.Note)] = p.Note,
                [nameof(PanjabiOrder.OrderId)] = p.OrderId
            }).ToList(),
            [nameof(NewOrderModel.ArabianOrders)] = o.ArabianOrders.Select(a => new Dictionary<string, object>
            {
                [nameof(ArabianOrder.Id)] = a.Id,
                [nameof(ArabianOrder.Amount)] = a.Amount,
                [nameof(ArabianOrder.Quantity)] = a.Quantity,
                [nameof(ArabianOrder.Length)] = s_decimalConv.ToFirestore(a.Length),
                [nameof(ArabianOrder.Tira)] = s_decimalConv.ToFirestore(a.Tira),
                [nameof(ArabianOrder.Hata)] = s_decimalConv.ToFirestore(a.Hata),
                [nameof(ArabianOrder.Ber)] = s_decimalConv.ToFirestore(a.Ber),
                [nameof(ArabianOrder.Cuff)] = s_decimalConv.ToFirestore(a.Cuff),
                [nameof(ArabianOrder.Mohori)] = s_decimalConv.ToFirestore(a.Mohori),
                [nameof(ArabianOrder.Komor)] = s_decimalConv.ToFirestore(a.Komor),
                [nameof(ArabianOrder.Rakaba)] = s_decimalConv.ToFirestore(a.Rakaba),
                [nameof(ArabianOrder.Ness)] = s_decimalConv.ToFirestore(a.Ness),
                [nameof(ArabianOrder.Note)] = a.Note,
                [nameof(ArabianOrder.OrderId)] = a.OrderId
            }).ToList(),
            [nameof(NewOrderModel.SelowerOrders)] = o.SelowerOrders.Select(s => new Dictionary<string, object>
            {
                [nameof(SelowerOrder.Id)] = s.Id,
                [nameof(SelowerOrder.Amount)] = s.Amount,
                [nameof(SelowerOrder.Quantity)] = s.Quantity,
                [nameof(SelowerOrder.Length)] = s_decimalConv.ToFirestore(s.Length),
                [nameof(SelowerOrder.Hip)] = s_decimalConv.ToFirestore(s.Hip),
                [nameof(SelowerOrder.Komor)] = s_decimalConv.ToFirestore(s.Komor),
                [nameof(SelowerOrder.Ness)] = s_decimalConv.ToFirestore(s.Ness),
                [nameof(SelowerOrder.Note)] = s.Note,
                [nameof(SelowerOrder.OrderId)] = s.OrderId
            }).ToList()
        };
    }

    private static NewOrderModel FromDoc(IDictionary<string, object> d)
    {
        var model = new NewOrderModel
        {
            Id = d.TryGetValue(nameof(NewOrderModel.Id), out var id) ? Convert.ToInt32(id) : 0,
            SL = d.TryGetValue(nameof(NewOrderModel.SL), out var sl) ? sl?.ToString() ?? string.Empty : string.Empty,
            CustomerName = d.TryGetValue(nameof(NewOrderModel.CustomerName), out var cn) ? cn?.ToString() ?? string.Empty : string.Empty,
            MobileNumber = d.TryGetValue(nameof(NewOrderModel.MobileNumber), out var mn) ? mn?.ToString() ?? string.Empty : string.Empty,
            Address = d.TryGetValue(nameof(NewOrderModel.Address), out var ad) ? ad?.ToString() ?? string.Empty : string.Empty,
            OrderDate = d.TryGetValue(nameof(NewOrderModel.OrderDate), out var od) ? FromTs(od) : DateTime.Now,
            DeliveryDate = d.TryGetValue(nameof(NewOrderModel.DeliveryDate), out var dd) ? FromTs(dd) : DateTime.Now,
            OrderFor = d.TryGetValue(nameof(NewOrderModel.OrderFor), out var ofor) ? ofor?.ToString() ?? "" : "",
            PaidAmount = d.TryGetValue(nameof(NewOrderModel.PaidAmount), out var pa) ? Convert.ToInt32(pa) : 0,
            DueAmount = d.TryGetValue(nameof(NewOrderModel.DueAmount), out var da) ? Convert.ToInt32(da) : 0,
            TotalAmount = d.TryGetValue(nameof(NewOrderModel.TotalAmount), out var ta) ? Convert.ToInt32(ta) : 0,
            Status = d.TryGetValue(nameof(NewOrderModel.Status), out var st) && Enum.TryParse<OrderStatus>(st?.ToString(), out var status) ? status : OrderStatus.Pending,
        };

        // Panjabi
        if (d.TryGetValue(nameof(NewOrderModel.PanjabiOrders), out var pList) && pList is IEnumerable<object> pObjs)
        {
            foreach (var obj in pObjs)
            {
                if (obj is IDictionary<string, object> pd)
                {
                    model.PanjabiOrders.Add(new PanjabiOrder
                    {
                        Id = pd.TryGetValue(nameof(PanjabiOrder.Id), out var pid) ? Convert.ToInt32(pid) : 0,
                        Amount = pd.TryGetValue(nameof(PanjabiOrder.Amount), out var pamt) ? Convert.ToInt32(pamt) : 0,
                        Quantity = pd.TryGetValue(nameof(PanjabiOrder.Quantity), out var pq) ? Convert.ToInt32(pq) : 0,
                        Length = pd.TryGetValue(nameof(PanjabiOrder.Length), out var pl) ? s_decimalConv.FromFirestore(pl) : 0,
                        Sina = pd.TryGetValue(nameof(PanjabiOrder.Sina), out var ps) ? s_decimalConv.FromFirestore(ps) : 0,
                        Komor = pd.TryGetValue(nameof(PanjabiOrder.Komor), out var pk) ? s_decimalConv.FromFirestore(pk) : 0,
                        Hata = pd.TryGetValue(nameof(PanjabiOrder.Hata), out var ph) ? s_decimalConv.FromFirestore(ph) : 0,
                        Cuff = pd.TryGetValue(nameof(PanjabiOrder.Cuff), out var pc) ? s_decimalConv.FromFirestore(pc) : 0,
                        Mohori = pd.TryGetValue(nameof(PanjabiOrder.Mohori), out var pm) ? s_decimalConv.FromFirestore(pm) : 0,
                        Rakaba = pd.TryGetValue(nameof(PanjabiOrder.Rakaba), out var pr) ? s_decimalConv.FromFirestore(pr) : 0,
                        Note = pd.TryGetValue(nameof(PanjabiOrder.Note), out var pn) ? pn?.ToString() ?? string.Empty : string.Empty,
                        OrderId = pd.TryGetValue(nameof(PanjabiOrder.OrderId), out var poid) ? Convert.ToInt32(poid) : 0
                    });
                }
            }
        }

        // Arabian
        if (d.TryGetValue(nameof(NewOrderModel.ArabianOrders), out var aList) && aList is IEnumerable<object> aObjs)
        {
            foreach (var obj in aObjs)
            {
                if (obj is IDictionary<string, object> adic)
                {
                    model.ArabianOrders.Add(new ArabianOrder
                    {
                        Id = adic.TryGetValue(nameof(ArabianOrder.Id), out var aid) ? Convert.ToInt32(aid) : 0,
                        Amount = adic.TryGetValue(nameof(ArabianOrder.Amount), out var aamt) ? Convert.ToInt32(aamt) : 0,
                        Quantity = adic.TryGetValue(nameof(ArabianOrder.Quantity), out var aq) ? Convert.ToInt32(aq) : 0,
                        Length = adic.TryGetValue(nameof(ArabianOrder.Length), out var al) ? s_decimalConv.FromFirestore(al) : 0,
                        Tira = adic.TryGetValue(nameof(ArabianOrder.Tira), out var at) ? s_decimalConv.FromFirestore(at) : 0,
                        Hata = adic.TryGetValue(nameof(ArabianOrder.Hata), out var ah) ? s_decimalConv.FromFirestore(ah) : 0,
                        Ber = adic.TryGetValue(nameof(ArabianOrder.Ber), out var ab) ? s_decimalConv.FromFirestore(ab) : 0,
                        Cuff = adic.TryGetValue(nameof(ArabianOrder.Cuff), out var ac) ? s_decimalConv.FromFirestore(ac) : 0,
                        Mohori = adic.TryGetValue(nameof(ArabianOrder.Mohori), out var amo) ? s_decimalConv.FromFirestore(amo) : 0,
                        Komor = adic.TryGetValue(nameof(ArabianOrder.Komor), out var ak) ? s_decimalConv.FromFirestore(ak) : 0,
                        Rakaba = adic.TryGetValue(nameof(ArabianOrder.Rakaba), out var ar) ? s_decimalConv.FromFirestore(ar) : 0,
                        Ness = adic.TryGetValue(nameof(ArabianOrder.Ness), out var an) ? s_decimalConv.FromFirestore(an) : 0,
                        Note = adic.TryGetValue(nameof(ArabianOrder.Note), out var anot) ? anot?.ToString() ?? string.Empty : string.Empty,
                        OrderId = adic.TryGetValue(nameof(ArabianOrder.OrderId), out var aoid) ? Convert.ToInt32(aoid) : 0
                    });
                }
            }
        }

        // Selower
        if (d.TryGetValue(nameof(NewOrderModel.SelowerOrders), out var sList) && sList is IEnumerable<object> sObjs)
        {
            foreach (var obj in sObjs)
            {
                if (obj is IDictionary<string, object> sd)
                {
                    model.SelowerOrders.Add(new SelowerOrder
                    {
                        Id = sd.TryGetValue(nameof(SelowerOrder.Id), out var sid) ? Convert.ToInt32(sid) : 0,
                        Amount = sd.TryGetValue(nameof(SelowerOrder.Amount), out var samt) ? Convert.ToInt32(samt) : 0,
                        Quantity = sd.TryGetValue(nameof(SelowerOrder.Quantity), out var sq) ? Convert.ToInt32(sq) : 0,
                        Length = sd.TryGetValue(nameof(SelowerOrder.Length), out var slen) ? s_decimalConv.FromFirestore(slen) : 0,
                        Hip = sd.TryGetValue(nameof(SelowerOrder.Hip), out var ship) ? s_decimalConv.FromFirestore(ship) : 0,
                        Komor = sd.TryGetValue(nameof(SelowerOrder.Komor), out var sk) ? s_decimalConv.FromFirestore(sk) : 0,
                        Ness = sd.TryGetValue(nameof(SelowerOrder.Ness), out var sn) ? s_decimalConv.FromFirestore(sn) : 0,
                        Note = sd.TryGetValue(nameof(SelowerOrder.Note), out var snot) ? snot?.ToString() ?? string.Empty : string.Empty,
                        OrderId = sd.TryGetValue(nameof(SelowerOrder.OrderId), out var soid) ? Convert.ToInt32(soid) : 0
                    });
                }
            }
        }

        return model;
    }

    private static List<NewOrderModel> Normalize(List<NewOrderModel> orders)
        => [.. orders.OrderByDescending(o => o.Id)];

    private static async Task<int> GetNextOrderIdAsync(FirestoreDb db, CancellationToken ct = default)
    {
        var countersRef = db.Document(CountersDoc);
        int newId = 0;
        await db.RunTransactionAsync(async transaction =>
        {
            var snap = await transaction.GetSnapshotAsync(countersRef);
            int lastId = 0;
            if (snap.Exists && snap.TryGetValue("lastOrderId", out int v)) lastId = v;
            newId = lastId + 1;
            transaction.Set(countersRef, new Dictionary<string, object> { ["lastOrderId"] = newId }, SetOptions.MergeAll);
        }, cancellationToken: ct);
        return newId;
    }

    private static async Task<int> GetWeekNextSerialAsync(FirestoreDb db, CancellationToken ct = default)
    {
        var (start, end) = GetCurrentWeekUtcRange();
        var q = db.Collection(OrdersCollection)
            .WhereGreaterThanOrEqualTo(nameof(NewOrderModel.OrderDate), ToTs(start))
            .WhereLessThan(nameof(NewOrderModel.OrderDate), ToTs(end));
        var snap = await q.GetSnapshotAsync(ct);
        return snap.Count + 1;
    }

    public async Task<bool> CreateOrder(NewOrderModel order)
    {
        try
        {
            var db = await GetDbAsync();

            order.Id = await GetNextOrderIdAsync(db);
            order.SL = GenerateOrderSerial.GetSL(await GetWeekNextSerialAsync(db));

            order.OrderDate = order.OrderDate.ToUniversalTime();
            order.DeliveryDate = order.DeliveryDate.ToUniversalTime();

            var docRef = db.Collection(OrdersCollection).Document(order.Id.ToString());
            await docRef.SetAsync(ToDoc(order));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteOrder(int id)
    {
        try
        {
            var db = await GetDbAsync();
            var docRef = db.Collection(OrdersCollection).Document(id.ToString());
            await docRef.DeleteAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<List<CustomerVM>> GetAllCustomers()
    {
        try
        {
            var db = await GetDbAsync();
            var snap = await db.Collection(OrdersCollection).GetSnapshotAsync();
            var orders = new List<NewOrderModel>(snap.Count);
            foreach (var doc in snap.Documents)
                orders.Add(FromDoc(doc.ToDictionary()));

            return [.. orders
                .GroupBy(o => o.MobileNumber)
                .Select(g => g.OrderByDescending(o => o.Id).First())
                .Select(o => new CustomerVM
                {
                    CustomerName = o.CustomerName,
                    Address = o.Address,
                    MobileNumber = o.MobileNumber
                })];
        }
        catch { return []; }
    }

    public async Task<List<NewOrderModel>> GetAllOrders()
    {
        try
        {
            var db = await GetDbAsync();
            var q = db.Collection(OrdersCollection).OrderByDescending(nameof(NewOrderModel.Id));
            var snap = await q.GetSnapshotAsync();
            var orders = new List<NewOrderModel>(snap.Count);
            foreach (var doc in snap.Documents)
                orders.Add(FromDoc(doc.ToDictionary()));
            return Normalize(orders);
        }
        catch { return []; }
    }

    public async Task<List<NewOrderModel>> GetCustomerOrders(string mobileNumber)
    {
        try
        {
            var db = await GetDbAsync();
            var q = db.Collection(OrdersCollection)
                .WhereEqualTo(nameof(NewOrderModel.MobileNumber), mobileNumber)
                .OrderByDescending(nameof(NewOrderModel.Id));
            var snap = await q.GetSnapshotAsync();
            var orders = new List<NewOrderModel>(snap.Count);
            foreach (var doc in snap.Documents)
                orders.Add(FromDoc(doc.ToDictionary()));
            return orders;
        }
        catch { return []; }
    }

    public async Task<NewOrderModel> GetOrder(int id)
    {
        try
        {
            var db = await GetDbAsync();
            var doc = await db.Collection(OrdersCollection).Document(id.ToString()).GetSnapshotAsync();
            if (!doc.Exists) return new NewOrderModel();
            return FromDoc(doc.ToDictionary());
        }
        catch { return new NewOrderModel(); }
    }

    public async Task<OrderSummaryVM> GetOrderSummary()
    {
        try
        {
            var db = await GetDbAsync();
            var snap = await db.Collection(OrdersCollection).GetSnapshotAsync();
            var orders = new List<NewOrderModel>(snap.Count);
            foreach (var doc in snap.Documents)
                orders.Add(FromDoc(doc.ToDictionary()));

            var utcNow = DateTime.UtcNow;
            var month = utcNow.Month;
            var year = utcNow.Year;
            var (startOfWeek, endOfWeek) = GetCurrentWeekUtcRange();

            var summary = new OrderSummaryVM
            {
                TotalOrders = orders.Count,
                TotalCustomers = orders.Select(o => o.MobileNumber).Distinct().Count(),
                MonthTotalOrders = orders.Count(o => o.OrderDate.ToUniversalTime().Month == month && o.OrderDate.ToUniversalTime().Year == year),
                WeekTotalOrders = orders.Count(o => o.OrderDate.ToUniversalTime().Date >= startOfWeek && o.OrderDate.ToUniversalTime().Date < endOfWeek),
                ReadyToDelivery = orders.Count(o => o.Status == OrderStatus.Completed)
            };

            return summary;
        }
        catch
        {
            return new OrderSummaryVM();
        }
    }

    public async Task<bool> UpdateOrder(NewOrderModel order)
    {
        try
        {
            var db = await GetDbAsync();
            var docRef = db.Collection(OrdersCollection).Document(order.Id.ToString());
            order.OrderDate = order.OrderDate.ToUniversalTime();
            order.DeliveryDate = order.DeliveryDate.ToUniversalTime();
            await docRef.SetAsync(ToDoc(order), SetOptions.Overwrite);
            return true;
        }
        catch { return false; }
    }
}
