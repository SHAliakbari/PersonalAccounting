using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace PersonalAccounting.BlazorApp.Components.Receipt_Component.Pages;

public static class ImageProcessingHelper
{
    public static byte[] GetBlackWhiteImage(Stream resourceImage)
    {
        try
        {
            resourceImage.Seek(0, SeekOrigin.Begin);
            using MemoryStream memoryStream = new MemoryStream();
            using (Image image = Image.Load(resourceImage))
            {
                image.Mutate(x => x
                    .Grayscale()
                    .BinaryThreshold(0.5f));
                image.Save(memoryStream, SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);
            }

            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            // Log error
            throw;
        }
    }

    public static byte[]? GetReducedImage(int width, int height, Stream resourceImage)
    {
        try
        {
            resourceImage.Seek(0, SeekOrigin.Begin);
            using MemoryStream memoryStream = new MemoryStream();
            using (Image image = Image.Load(resourceImage))
            {
                image.Mutate(x => x
                     .Resize(width, height)
                     .Grayscale());
                image.Save(memoryStream, SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);
            }

            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static string GetMimeTypeForFileExtension(string filePath)
    {
        const string DefaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(filePath, out string contentType))
        {
            contentType = DefaultContentType;
        }

        return contentType;
    }
}