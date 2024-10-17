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
            databaseId: ID.Unique(),
            name: "TailorDb"
        );

        var ordersCollection = await _databases.CreateCollection(
            databaseId: orderDatabase.Id,
            collectionId: ID.Unique(),
            name: "Orders"
        );

        await AddOrderAttributes(orderDatabase.Id, ordersCollection.Id);

        var arabianOrderCollection = await CreateArabianOrderCollection(orderDatabase.Id);
        var panjabiOrderCollection = await CreatePanjabiOrderCollection(orderDatabase.Id);
        var selowerOrderCollection = await CreateSelowerOrderCollection(orderDatabase.Id);
        await CreateRelationship(orderDatabase.Id, ordersCollection.Id, arabianOrderCollection.Id, "arabianOrder");
        await CreateRelationship(orderDatabase.Id, ordersCollection.Id, panjabiOrderCollection.Id, "panjabiOrder");
        await CreateRelationship(orderDatabase.Id, ordersCollection.Id, selowerOrderCollection.Id, "selowerOrder");

        Console.WriteLine("Database setup completed with relationships.");
    }

    private async Task AddOrderAttributes(string databaseId, string collectionId)
    {
        await _databases.CreateStringAttribute(databaseId, collectionId, "customerName", 255, required: true);
        await _databases.CreateStringAttribute(databaseId, collectionId, "mobileNumber", 20, required: true);
        await _databases.CreateStringAttribute(databaseId, collectionId, "address", 500, required: false);
        await _databases.CreateDatetimeAttribute(databaseId, collectionId, "orderDate", required: true);
        await _databases.CreateDatetimeAttribute(databaseId, collectionId, "deliveryDate", required: true);
        await _databases.CreateStringAttribute(databaseId, collectionId, "orderFor", 50, required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "paidAmount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "dueAmount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "totalAmount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collectionId, "status", required: true);
    }

    private async Task<Collection> CreateArabianOrderCollection(string databaseId)
    {
        var collection = await _databases.CreateCollection(
            databaseId: databaseId,
            collectionId: ID.Unique(),
            name: "ArabianOrder"
        );
        await _databases.CreateStringAttribute(databaseId, collection.Id, "orderId", 100, required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "amount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "quantity", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "length", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "tira", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "hata", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "cuff", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "mohori", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "rakaba", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "ness", required: true);
        await _databases.CreateStringAttribute(databaseId, collection.Id, "note", 500, required: false);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "assignTo", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "status", required: true);
        return collection;
    }

    private async Task<Collection> CreatePanjabiOrderCollection(string databaseId)
    {
        var collection = await _databases.CreateCollection(
            databaseId: databaseId,
            collectionId: ID.Unique(),
            name: "PanjabiOrder"
        );
        await _databases.CreateStringAttribute(databaseId, collection.Id, "orderId", 100, required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "amount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "quantity", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "length", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "sina", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "komor", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "hata", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "cuff", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "mohori", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "rakaba", required: true);
        await _databases.CreateStringAttribute(databaseId, collection.Id, "note", 500, required: false);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "assignTo", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "status", required: true);

        return collection;
    }

    private async Task<Collection> CreateSelowerOrderCollection(string databaseId)
    {
        var collection = await _databases.CreateCollection(
            databaseId: databaseId,
            collectionId: ID.Unique(),
            name: "SelowerOrder"
        );

        await _databases.CreateStringAttribute(databaseId, collection.Id, "orderId", 100, required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "amount", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "quantity", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "length", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "hip", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "komor", required: true);
        await _databases.CreateFloatAttribute(databaseId, collection.Id, "ness", required: true);
        await _databases.CreateStringAttribute(databaseId, collection.Id, "note", 500, required: false);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "assignTo", required: true);
        await _databases.CreateIntegerAttribute(databaseId, collection.Id, "status", required: true);

        return collection;
    }

    private async Task CreateRelationship(string databaseId, string parentCollectionId, string childCollectionId, string key)
    {
        await _databases.CreateRelationshipAttribute(
            databaseId: databaseId,
            collectionId: parentCollectionId,
            relatedCollectionId: childCollectionId,
            type: Appwrite.Enums.RelationshipType.OneToOne,
            key: key
        );
    }
}
