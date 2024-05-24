using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEdit.Models;

public sealed class Tile
{
    public int Width => Bitmap.Width;

    public int Height => Bitmap.Height;

    public SKBitmap Bitmap { get; private set; } 

    public void SaveToStream(Stream stream, SKEncodedImageFormat format, int quality)
    {
        using var image = SKImage.FromBitmap(Bitmap);
        using var data = image.Encode(format, quality);        
        data.SaveTo(stream);
    }

    public void SaveToStream(Stream stream, SKEncodedImageFormat format)
    {
        SaveToStream(stream, format, 100);
    }

    public void SaveToStream(Stream stream)
    {
        SaveToStream(stream, SKEncodedImageFormat.Png);
    }

    public Tile(int width, int height, SKColorType colorType, SKAlphaType alphaType, SKColorSpace colorSpace)
    {
       Bitmap = new SKBitmap(width, height, colorType, alphaType, colorSpace);
    }

    public Tile(SKImage image)
    {
        Bitmap = new SKBitmap(image.Info);
    }

    public static Tile LoadFromStream(Stream stream)
    {
        var image = SKImage.FromEncodedData(stream);
        return new Tile(image);
    }
}
