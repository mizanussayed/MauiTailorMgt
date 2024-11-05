using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
namespace MYPM.Configurations;
public class AppWriteDatabaseSetup(Client client)
{
    private readonly Databases _databases = new(client);

    public async Task SetupDatabase()
    {
        var orderDatabase = await _databases.Create(
            databaseId: "db_tailor_0001",
            name: "TailorDb"
        );

        var ordersCollection = await _databases.CreateCollection(
            databaseId: orderDatabase.Id,
            collectionId: "tbl_order_0001",
            name: "Orders"
        );

        await AddOrderAttributes(orderDatabase.Id, ordersCollection.Id);
        await AddArabianAttributes(orderDatabase.Id, ordersCollection.Id);
        await AddPanjabiAttributes(orderDatabase.Id, ordersCollection.Id);
        await AddSelowerAttributes(orderDatabase.Id, ordersCollection.Id);
        //await CreateRelationship(orderDatabase.Id, ordersCollection.Id, selowerOrderCollection.Id, "selowerOrder");
        Console.WriteLine("Database setup completed with relationships.");
    }

    private async Task AddOrderAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateStringAttribute(databaseId, collectionId, "customerName", 255, required: true);
        await _databases.CreateStringAttribute(databaseId, collectionId, "mobileNumber", 15, required: true);
        await _databases.CreateStringAttribute(databaseId, collectionId, "address", 500, required: false);
        await _databases.CreateDatetimeAttribute(databaseId, collectionId, "orderDate", required: true);
        await _databases.CreateDatetimeAttribute(databaseId, collectionId, "deliveryDate", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "paidAmount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "dueAmount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "totalAmount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "status", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "assignTo", required: false);
    }

    private async Task AddArabianAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "arabian.amount", required: false);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "arabian.quantity", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.length", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.tira", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.hata", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.cuff", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.mohori", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.rakaba", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabian.ness", required: false);
        await _databases.CreateStringAttribute(databaseId, collectionId, "arabian.note", 500, required: false);
    }

    private async Task AddPanjabiAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "panjabi.amount", required: false);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "panjabi.quantity", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.length", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.sina", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.komor", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.hata", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.cuff", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.mohori", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabi.rakaba", required: false);
        await _databases.CreateStringAttribute(databaseId, collectionId, "panjabi.note", 500, required: false);
    }

    private async Task AddSelowerAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "selower.amount", required: false);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "selower.quantity", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selower.length", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selower.hip", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selower.komor", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selower.ness", required: false);
        await _databases.CreateStringAttribute(databaseId, collectionId, "selower.note", 500, required: false);
    }
}
