using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
using MYPM.Configurations;



namespace MYPM.Services;

public class GalleryService
{
    private readonly Storage _storage;

    public GalleryService()
    {
        _storage = new AppWriteClient().GetStorage();
    }
    public async Task<string> UploadGalleryImages(Stream fileStream, string fileName)
    {
        try
        {
            var bucketId = "673104ef0029440a9c01";
            
            var file = InputFile.FromStream(fileStream, fileName, "image/png");
            var response = await _storage.CreateFile(bucketId, ID.Unique(), file).ConfigureAwait(false);
            var fileId = response.Id;

            var imageUrl = $"https://your-appwrite-instance.com/v1/storage/buckets/{bucketId}/files/{fileId}/view";

            return imageUrl;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error uploading image: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task DeleteImage(string fileId)
    {
        try
        {
            var bucketId = "673104ef0029440a9c01";
            await _storage.DeleteFile(bucketId, fileId);
        }
        catch (Exception)
        {
            Console.WriteLine("Error deleting image");
        }
    }
    public async Task<FileList> GetImages()
    {
        try
        {
            var bucketId = "673104ef0029440a9c01";
            var files = await _storage.ListFiles(bucketId);
            return files;
        }
        catch (Exception)
        {
            return new FileList(0, []);
        }
    }

}


