//using SkiaSharp;

//using;

//namespace MYPM.Common;
//public class ImageExtention
//{
//    public static Image GenerateQRCode(string text)
//    {
//        var writer = new BarcodeWriter<Image>
//        {
//            Format = BarcodeFormat.QR_CODE,
//            Options = new ZXing.Common.EncodingOptions
//            {
//                Width = 200,
//                Height = 200
//            }
//        };
//        return writer.Write(text);
//    }
//}
//public static async Task<string> RenderContentViewToPng(ContentView contentView)
//    //{

//    //    var width = (int)contentView.Width;
//    //    var height = (int)contentView.Height;

//    //    // Create a new bitmap
//    //    using var bitmap = new SKBitmap(width, height);
//    //    using var canvas = new SKCanvas(bitmap);

//    //    // Clear the canvas
//    //    canvas.Clear(SKColors.White);

//    //    // Render the content view to the canvas
//    //    var image = await contentView.Dispatcher.Dispatch(() =>
//    //    {
//    //        var renderer = new SKXmlWriter(contentView);
//    //        return renderer.RenderToImage(contentView, width, height);
//    //    });

//    //    canvas.DrawImage(image, 0, 0);

//    //    // Save the bitmap as PNG
//    //    var pngPath = Path.Combine(FileSystem.CacheDirectory, "sharedContent.png");
//    //    using (var stream = new FileStream(pngPath, FileMode.Create, FileAccess.Write))
//    //    {
//    //        bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
//    //    }

//    //    return pngPath;
//    //}
//    public async Task SaveToImageAsync(string filePath)
//    {
//        // Render the page to an image
//        var streamImageSource = new StreamImageSource
//        {
//            Stream = async (c) =>
//            {
//                // Create a new Bitmap with the size of your ContentView
//                var width = (int)Width;
//                var height = (int)Height;
//                var bitmap = new Bitmap(width, height);

//                // Capture the view into the bitmap
//                await bitmap.RenderAsync(this);

//                // Save the bitmap to a stream
//                var memoryStream = new MemoryStream();
//                await bitmap.SaveAsync(memoryStream, BitmapFormat.Png);
//                memoryStream.Position = 0;

//                return memoryStream;
//            }
//        };

//        // Load the image and save it to a file
//       // var imageSource = await streamImageSource.GetImageSourceAsync();
//        using (var fileStream = new FileStream(filePath, FileMode.Create))
//        {
//            await imageSource.CopyToAsync(fileStream);
//        }
//    }




//    public static async Task SharePngFile(string filePath)
//    {
//        var shareMessage = new ShareFileRequest
//        {
//            Title = "Share Invoice PNG",
//            File = new ShareFile(filePath)
//        };

//        await Share.RequestAsync(shareMessage);
//    }








