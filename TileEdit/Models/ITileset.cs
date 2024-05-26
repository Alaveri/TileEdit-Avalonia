using SkiaSharp;
using System.IO;

namespace TileEdit.Models;
public interface ITileset
{
    /// <summary>
    /// The list of tiles in the tileset.
    /// </summary>
    ITile this[int index] { get; }

    /// <summary>
    /// The AlphaType to use for new tiles.
    /// </summary>
    SKAlphaType AlphaType { get; }

    /// <summary>
    /// The ColorSpace to use for new tiles.
    /// </summary>
    SKColorSpace ColorSpace { get; }

    /// <summary>
    /// The ColorType to use for new tiles.
    /// </summary>
    TilesetPixelFormat PixelFormat { get; }

    /// <summary>
    /// The type of tileset dimensions, Fixed or Variable.
    /// </summary>
    TilesetDimensions Dimensions { get; }

    /// <summary>
    /// The number of tiles in the tileset.
    /// </summary>
    int TileCount { get; }

    /// <summary>
    /// The width of the tiles in the tileset.
    /// </summary>
    int TileHeight { get; }

    /// <summary>
    /// The height of the tiles in the tileset.
    /// </summary>
    int TileWidth { get; }

    /// <summary>
    /// Adds a new tile to the tileset.
    /// </summary>
    /// <param name="pixelFormat">The pixel format of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile</param>
    /// <returns>A new <see cref="Tile"></see> object with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called on a variable tileset.</exception>
    ITile AddTile(TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null);

    /// <summary>
    /// Adds a new tile to the tileset with the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the tile.</param>
    /// <param name="height">The height of the tile.</param>
    /// <param name="pixelFormat">The pixel format of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile</param>
    /// <returns>A new <see cref="Tile" /> object with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called on a fixed tileset.</exception>
    ITile AddTile(int width, int height, TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null);

    /// <summary>
    /// Removes a tile from the tileset.
    /// </summary>
    /// <param name="tile">The tile to remove.</param>
    void ClearTiles();

    /// <summary>
    /// Inserts a new tile at the specified index.
    /// </summary>
    /// <param name="index">The index of the tile.</param>
    /// <param name="pixelFormat">The pixel format of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile</param>
    /// <returns>A new <see cref="ITile" /> object with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called on a variable tileset.</exception>
    ITile InsertTile(int index, TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null);

    /// <summary>
    /// Inserts a new tile at the specified index.
    /// </summary>
    /// <param name="index">The index of the tile.</param>
    /// <param name="width">The width of the tile.</param>
    /// <param name="height">The height of the tile.</param>
    /// <param name="pixelFormat">The pixel format of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile</param>
    /// <returns>A new <see cref="ITile"></see> object with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called on a fixed tileset.</exception>
    ITile InsertTile(int index, int width, int height, TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null);

    /// <summary>
    /// Moves a tile from one index to another.
    /// </summary>
    /// <param name="fromIndex">The index of the tile to move.</param>
    /// <param name="toIndex">The new index of the tile.</param>
    void MoveTile(int fromIndex, int toIndex);

    /// <summary>
    /// Removes a tile from the tileset.
    /// </summary>
    /// <param name="index">The index of the tile to remove.</param>
    void RemoveTile(int index);

    /// <summary>
    /// Removes a tile from the tileset.
    /// </summary>
    /// <param name="tile">The tile to remove.</param>
    void RemoveTile(Tile tile);

    /// <summary>
    /// Saves the tileset to the specified file as a full quality PNG.
    /// </summary>
    /// <param name="filename">The name of the tileset file.</param>
    void SaveToFile(string filename);

    /// <summary>
    /// Saves the tileset to the specified file using the specified format and quality.
    /// </summary>
    /// <param name="filename">The name of the tileset file.</param>
    /// <param name="format">The format of the tile images.</param>
    /// <param name="quality">The quality of the tile images.</param>
    void SaveToFile(string filename, SKEncodedImageFormat format, int quality = 100);

    /// <summary>
    /// Saves the tileset to the specified stream as a full quality PNG.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    void SaveToStream(Stream stream);

    /// <summary>
    /// Saves the tileset to the specified stream using the specified format and quality.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="format">The format of the tile images.</param>
    /// <param name="quality">The quality of the tile images.</param>
    void SaveToStream(Stream stream, SKEncodedImageFormat format, int quality = 100);

    /// <summary>
    /// Swaps the positions of two tiles in the tileset.
    /// </summary>
    /// <param name="index1">The index of the first tile.</param>
    /// <param name="index2">The index of the second tile.</param>
    void SwapTiles(int index1, int index2);
}