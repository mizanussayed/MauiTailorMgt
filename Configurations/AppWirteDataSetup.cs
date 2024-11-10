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
        //await CreateRelationship(orderDatabase.Id, ordersCollection.Id, selowerrderCollection.Id, "selowerrder");
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
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "arabianAmount", required: false);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "arabianQuantity", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianLength", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianTira", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianHata", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianCuff", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianMohori", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianRakaba", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "arabianNess", required: false);
        await _databases.CreateStringAttribute(databaseId, collectionId, "arabianNote", 500, required: false);
    }

    private async Task AddPanjabiAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "panjabiAmount", required: false);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "panjabiQuantity", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiLength", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiSina", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiKomor", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiHata", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiCuff", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiMohori", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "panjabiRakaba", required: false);
        await _databases.CreateStringAttribute(databaseId, collectionId, "panjabiNote", 500, required: false);
    }

    private async Task AddSelowerAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "selowerAmount", required: false);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "selowerQuantity", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selowerLength", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selowerHip", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selowerKomor", required: false);
        await _databases.CreateFloatAttribute(databaseId, collectionId, "selowerNess", required: false);
        await _databases.CreateStringAttribute(databaseId, collectionId, "selowerNote", 500, required: false);
    }
}
