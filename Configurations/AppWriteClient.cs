using Appwrite;
using Appwrite.Models;
using Appwrite.Services;

namespace MYPM.Configurations;

public class AppWriteClient
{
    private readonly Databases _databases;
    public AppWriteClient()
    {
        var _client = new Client()
             .SetEndpoint("https://cloud.appwrite.io/v1")
             .SetProject("670d59350035da764949")
             .SetKey("standard_a790178aff29200de625d8fb62d0d10bfabda79345c11568590c0d2e2ca6689803ca131e56380d7c83569686323f7f3974662672c3d358da85c9dfe12b46dbe0fa7ff32a93dc3c62eef3593d884636f68a3cd32c6b266e60de9d119cbe55f01650269c71db26735fb4649e25f63d2d8cce2a68b675ad7ebb353fa4cdeea145cc");
        _databases = new Databases(_client);
        //var setup = new AppWriteDatabaseSetup(_client);
        //setup.SetupDatabase();
    }
    public Databases GetDatabase() => _databases;
    public static string DatabaseId() => "db_tailor_0001";
    public static string OrderCollectionId() => "tbl_order_0001";
}




