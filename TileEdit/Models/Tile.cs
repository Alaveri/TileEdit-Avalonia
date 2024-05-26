using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEdit.Models;

public sealed class Tile : ITile
{
    /// <summary>
    /// The width of the tile.
    /// </summary>
    public int Width => Bitmap.Width;

    /// <summary>
    /// The height of the tile.
    /// </summary>
    public int Height => Bitmap.Height;

    /// <summary>
    /// The bitmap data of the tile.
    /// </summary>
    public SKBitmap Bitmap { get; private set; }

    /// <summary>
    /// Saves the tile to the specified stream using the specified format and quality.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="format">The format of the tile.</param>
    /// <param name="quality">The quality of the tile data.</param>
    public void SaveToStream(Stream stream, SKEncodedImageFormat format, int quality = 100)
    {
        using var image = SKImage.FromBitmap(Bitmap);
        using var data = image.Encode(format, quality);
        data.SaveTo(stream);
    }

    /// <summary>
    /// Saves the tile to the specified stream using the specified format with 100 percent quality.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="format">The format of the tile.</param>
    public void SaveToStream(Stream stream, SKEncodedImageFormat format)
    {
        SaveToStream(stream, format);
    }

    /// <summary>
    /// Saves the tile to the specified stream using as a full quality PNG.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    public void SaveToStream(Stream stream)
    {
        SaveToStream(stream, SKEncodedImageFormat.Png);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tile"/> class with the specified properties.
    /// </summary>
    /// <param name="width">The width of the tile.</param>
    /// <param name="height">The height of the tile.</param>
    /// <param name="colorType">The ColorType of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile.</param>
    public Tile(int width, int height, SKColorType colorType, SKAlphaType alphaType, SKColorSpace colorSpace)
    {
        Bitmap = new SKBitmap(width, height, colorType, alphaType, colorSpace);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ITile"/> using the specified Skia image.
    /// </summary>
    /// <param name="image">The source image.</param>
    public Tile(SKImage image)
    {
        Bitmap = new SKBitmap(image.Info);
    }

    /// <summary>
    /// Loads a tile from the specified stream.
    /// </summary>
    /// <param name="stream">The source stream.</param>
    /// <returns>A new <see cref="ITile"/> object loaded from the stream.</returns>
    public static ITile LoadFromStream(Stream stream)
    {
        var image = SKImage.FromEncodedData(stream);
        return new Tile(image);
    }
}
