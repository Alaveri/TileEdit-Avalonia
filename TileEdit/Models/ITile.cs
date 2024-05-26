using SkiaSharp;
using System.IO;

namespace TileEdit.Models;
public interface ITile
{
    /// <summary>
    /// The bitmap data of the tile.
    /// </summary>
    SKBitmap Bitmap { get; }

    /// <summary>
    /// The width of the tile.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// The height of the tile.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Saves the tile to the specified stream using as a full quality PNG.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    void SaveToStream(Stream stream);

    /// <summary>
    /// Saves the tile to the specified stream using the specified format and 100 percent quality.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    void SaveToStream(Stream stream, SKEncodedImageFormat format);

    /// <summary>
    /// Saves the tile to the specified stream using the specified format and quality.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="format">The format of the tile.</param>
    /// <param name="quality">The quality of the tile data.</param>
    void SaveToStream(Stream stream, SKEncodedImageFormat format, int quality);
}