
using MYPM.Services;

namespace MYPM.ViewModels;

public partial class GalleryViewModel : ObservableObject
{


    [ObservableProperty]
    private ObservableCollection<ImageItem> _images =  [];
    public GalleryViewModel()
    {
        LoadImages().ConfigureAwait(false);
    }

    private async Task LoadImages()
    {
        try
        {
            var service = new GalleryService();
            var files = await service.GetImages();
            foreach (var file in files.Files)
            {
                Images.Add(new ImageItem
                {
                    ImageUrl = "https://your-appwrite-instance.com/v1/storage/buckets/your-bucket-id/files/" + file.Id + "/view"
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading images: " + ex.Message);
        }
    }

    public async Task UploadImageAsync(Stream imageStream, string fileName)
    {
        try
        {
            var service = new GalleryService();
            var fileUrl = await service.UploadGalleryImages(imageStream, fileName);
            Images.Add(new ImageItem { ImageUrl = fileUrl });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error uploading image: " + ex.Message);
        }
    }
}

public class ImageItem
{
    public string ImageUrl { get; set; } = string.Empty;
}
