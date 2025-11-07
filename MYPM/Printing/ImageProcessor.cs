namespace Printing;

/// <summary>
/// Image processing utilities for ESC/POS printing
/// </summary>
public static class ImageProcessor
{
    public static (byte[] data, int width, int height) ProcessImage(byte[] imageBytes, int maxWidth = 384)
    {
        return (Array.Empty<byte>(), 0, 0);
    }

    /// <summary>
    /// Convert a monochrome bitmap to ESC/POS format
    /// </summary>
    /// <param name="bitmap">Monochrome bitmap data (1 bit per pixel)</param>
    /// <param name="width">Image width in pixels</param>
    /// <param name="height">Image height in pixels</param>
    /// <returns>ESC/POS formatted image data</returns>
    public static byte[] ConvertToEscPos(byte[] bitmap, int width, int height)
    {
        // Calculate width in bytes
        int widthBytes = (width + 7) / 8;
        var result = new byte[widthBytes * height];

        // Pack bits into bytes (8 pixels per byte)
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < widthBytes; x++)
            {
                byte b = 0;
                for (int bit = 0; bit < 8; bit++)
                {
                    int pixelX = x * 8 + bit;
                    if (pixelX < width)
                    {
                        int index = y * width + pixelX;
                        if (index < bitmap.Length && bitmap[index] > 0)
                        {
                            b |= (byte)(0x80 >> bit);
                        }
                    }
                }
                result[y * widthBytes + x] = b;
            }
        }

        return result;
    }

    /// <summary>
    /// Apply Floyd-Steinberg dithering to grayscale image
    /// </summary>
    public static byte[] ApplyDithering(byte[] grayscale, int width, int height)
    {
        var result = new byte[width * height];
        Array.Copy(grayscale, result, grayscale.Length);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                byte oldPixel = result[index];
                byte newPixel = (byte)(oldPixel > 127 ? 255 : 0);
                result[index] = newPixel;

                int error = oldPixel - newPixel;

                // Distribute error to neighboring pixels
                if (x + 1 < width)
                    result[index + 1] = Clamp(result[index + 1] + error * 7 / 16);

                if (y + 1 < height)
                {
                    if (x > 0)
                        result[index + width - 1] = Clamp(result[index + width - 1] + error * 3 / 16);

                    result[index + width] = Clamp(result[index + width] + error * 5 / 16);

                    if (x + 1 < width)
                        result[index + width + 1] = Clamp(result[index + width + 1] + error * 1 / 16);
                }
            }
        }

        return result;
    }

    private static byte Clamp(int value)
    {
        if (value < 0) return 0;
        if (value > 255) return 255;
        return (byte)value;
    }
}
