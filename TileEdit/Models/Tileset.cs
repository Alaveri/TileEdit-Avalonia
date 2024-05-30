using Alaveri.Core.Enumerations;
using Alaveri.Core.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TileEdit.Models;

public enum TilesetDimensions
{
    [EnumDescriptor(identifier: "FixedSize")]
    FixedSize,
    [EnumDescriptor(identifier: "VariableSize")]
    VariableSize
}

public enum TilesetPixelFormat
{
    [LinkedEnumDescriptor<SKColorType>(identifier: "Rgba8Bit", linkedEnumValue: SKColorType.Rgba8888)]
    Rgba8Bit,
    [LinkedEnumDescriptor<SKColorType>(identifier: "Rgba16Bit", linkedEnumValue: SKColorType.Argb4444)]
    Rgba16Bit,
    [LinkedEnumDescriptor<SKColorType>(identifier: "Gray8Bit", linkedEnumValue: SKColorType.Gray8)]
    Gray8Bit,
    [LinkedEnumDescriptor<SKColorType>(identifier: "Indexed8Bit", linkedEnumValue: SKColorType.Rgb888x)]
    Indexed8Bit,
    [LinkedEnumDescriptor<SKColorType>(identifier: "Indexed6Bit", linkedEnumValue: SKColorType.Rgb888x)]
    Indexed6Bit
}

public sealed class Tileset : ITileset
{
    /// <summary>
    /// The type of tileset dimensions, Fixed or Variable.
    /// </summary>
    public TilesetDimensions Dimensions { get; private set; } = TilesetDimensions.FixedSize;

    /// <summary>
    /// The list of tiles in the tileset.
    /// </summary>
    private IList<ITile> Tiles { get; } = [];

    /// <summary>
    /// The ColorSpace to use for new tiles.
    /// </summary>
    public SKColorSpace ColorSpace { get; private set; }

    /// <summary>
    /// The AlphaType to use for new tiles.
    /// </summary>
    public SKAlphaType AlphaType { get; private set; }

    /// <summary>
    /// The ColorType to use for new tiles.
    /// </summary>
    public TilesetPixelFormat PixelFormat { get; private set; }

    /// <summary>
    /// The width of the tiles in the tileset.
    /// </summary>
    public int TileWidth { get; private set; }

    /// <summary>
    /// The height of the tiles in the tileset.
    /// </summary>
    public int TileHeight { get; private set; }

    /// <summary>
    /// The number of tiles in the tileset.
    /// </summary>
    public int TileCount => Tiles.Count;

    /// <summary>
    /// Indexer for the tileset.
    /// </summary>
    /// <param name="index">The index of the tileset.</param>
    /// <returns>The tile at the specified index.</returns>
    public ITile this[int index] => Tiles[index];

    /// <summary>
    /// Initializes a new instance of the Tileset class with fixed dimensions.
    /// </summary>
    /// <param name="width">The width of the tiles in the tileset.</param>
    /// <param name="height">The height of the tiles in the tileset.</param>
    /// <param name="pixelFormat">The pixel format to use for new tiles.</param>
    /// <param name="alphaType">The AlphaType to use for new tiles.</param>
    /// <param name="colorSpace">The ColorSpace to use for new tiles.</param>
    public Tileset(int width, int height, TilesetPixelFormat pixelFormat, SKAlphaType alphaType = SKAlphaType.Premul, SKColorSpace? colorSpace = null)
    {
        colorSpace ??= SKColorSpace.CreateSrgb();
        ColorSpace = colorSpace;
        PixelFormat = pixelFormat;
        Dimensions = TilesetDimensions.FixedSize;
        TileWidth = width;
        TileHeight = height;
        AlphaType = alphaType;
    }

    /// <summary>
    /// Initializes a new instance of the Tileset class with variable dimensions.
    /// </summary>
    /// <param name="pixelFormat">The pixel format to use for new tiles.</param>
    /// <param name="alphaType">The AlphaType to use for new tiles.</param>
    /// <param name="colorSpace">The ColorSpace to use for new tiles.</param>
    public Tileset(TilesetPixelFormat pixelFormat, SKAlphaType alphaType = SKAlphaType.Premul, SKColorSpace? colorSpace = null)
    {
        colorSpace ??= SKColorSpace.CreateSrgb();
        ColorSpace = colorSpace;
        PixelFormat = pixelFormat;
        Dimensions = TilesetDimensions.VariableSize;
        TileWidth = 0;
        TileHeight = 0;
        AlphaType = alphaType;
    }

    /// <summary>
    /// Initializes a new instance of the Tileset class with default values.
    /// </summary>
    public Tileset()
    {
        ColorSpace = SKColorSpace.CreateSrgb();
        PixelFormat = TilesetPixelFormat.Rgba8Bit;
        Dimensions = TilesetDimensions.FixedSize;
        TileWidth = 0;
        TileHeight = 0;
        AlphaType = SKAlphaType.Premul;
    }

    /// <summary>
    /// Adds a new tile to the tileset.
    /// </summary>
    /// <param name="pixelFormat">The pixel format of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile</param>
    /// <returns>A new <see cref="Tile"></see> object with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called on a variable tileset.</exception>
    public ITile AddTile(TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.FixedSize)
            throw new InvalidOperationException("Cannot add a fixed tile to a variable tileset.");
        pixelFormat ??= PixelFormat;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(TileWidth, TileHeight, pixelFormat.Value.GetLinkedEnum<SKColorType>(), alphaType.Value, colorSpace);
        Tiles.Add(tile);
        return tile;
    }

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
    public ITile AddTile(int width, int height, TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.VariableSize)
            throw new InvalidOperationException("Cannot add a variable tile to a fixed tileset.");
        pixelFormat ??= PixelFormat;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(width, height, pixelFormat.Value.GetLinkedEnum<SKColorType>(), alphaType.Value, colorSpace);
        Tiles.Add(tile);
        return tile;
    }

    /// <summary>
    /// Removes a tile from the tileset.
    /// </summary>
    /// <param name="tile">The tile to remove.</param>
    public void RemoveTile(Tile tile)
    {
        Tiles.Remove(tile);
    }

    /// <summary>
    /// Clears all tiles from the tileset.
    /// </summary>

    public void ClearTiles()
    {
        Tiles.Clear();
    }

    /// <summary>
    /// Swaps the positions of two tiles in the tileset.
    /// </summary>
    /// <param name="index1">The index of the first tile.</param>
    /// <param name="index2">The index of the second tile.</param>
    public void SwapTiles(int index1, int index2)
    {
        (Tiles[index2], Tiles[index1]) = (Tiles[index1], Tiles[index2]);
    }

    /// <summary>
    /// Moves a tile from one index to another.
    /// </summary>
    /// <param name="fromIndex">The index of the tile to move.</param>
    /// <param name="toIndex">The new index of the tile.</param>
    public void MoveTile(int fromIndex, int toIndex)
    {
        var tile = Tiles[fromIndex];
        Tiles.RemoveAt(fromIndex);
        Tiles.Insert(toIndex, tile);
    }

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
    public ITile InsertTile(int index, int width, int height, TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.VariableSize)
            throw new InvalidOperationException("Cannot add a variable tile to a fixed tileset.");
        pixelFormat ??= PixelFormat;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(width, height, pixelFormat.Value.GetLinkedEnum<SKColorType>(), alphaType.Value, colorSpace);
        Tiles.Insert(index, tile);
        return tile;
    }

    /// <summary>
    /// Inserts a new tile at the specified index.
    /// </summary>
    /// <param name="index">The index of the tile.</param>
    /// <param name="pixelFormat">The pixel format of the tile.</param>
    /// <param name="alphaType">The AlphaType of the tile.</param>
    /// <param name="colorSpace">The ColorSpace of the tile</param>
    /// <returns>A new <see cref="ITile" /> object with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called on a variable tileset.</exception>
    public ITile InsertTile(int index, TilesetPixelFormat? pixelFormat = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.FixedSize)
            throw new InvalidOperationException("Cannot add a fixed width tile to a variable tileset.");
        pixelFormat ??= PixelFormat;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(TileWidth, TileHeight, pixelFormat.Value.GetLinkedEnum<SKColorType>(), alphaType.Value, colorSpace);
        Tiles.Insert(index, tile);
        return tile;
    }

    /// <summary>
    /// Removes a tile from the tileset.
    /// </summary>
    /// <param name="index">The index of the tile to remove.</param>
    public void RemoveTile(int index)
    {
        Tiles.RemoveAt(index);
    }

    /// <summary>
    /// Writes header information for the tileset to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to use for writing.</param>
    private void WriteHeader(Stream stream)
    {
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(TilesetConstants.MajorVersion);
        writer.Write(TilesetConstants.MinorVersion);
        writer.Write((int)Dimensions);
        writer.Write(TileWidth);
        writer.Write(TileHeight);
        writer.Write((int)PixelFormat);
        writer.Write((int)AlphaType);
        writer.Write(Tiles.Count);
    }

    /// <summary>
    /// Loads a tileset from the specified stream.
    /// </summary>
    /// <param name="stream">The source stream.</param>
    /// <returns>A new <see cref="ITileset" /> loaded from the specified stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the version of the tileset is not supported.</exception>
    public static ITileset LoadFromStream(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        var majorVersion = reader.ReadInt32();
        var minorVersion = reader.ReadInt32();
        if (majorVersion > TilesetConstants.MajorVersion || (majorVersion == TilesetConstants.MajorVersion && minorVersion > TilesetConstants.MinorVersion))
            throw new InvalidOperationException("Unsupported tileset version.");
        var tileset = new Tileset
        {
            Dimensions = (TilesetDimensions)reader.ReadInt32(),
            TileWidth = reader.ReadInt32(),
            TileHeight = reader.ReadInt32(),
            PixelFormat = (TilesetPixelFormat)reader.ReadInt32(),
            AlphaType = (SKAlphaType)reader.ReadInt32()
        };
        var tileCount = reader.ReadInt32();
        for (var index = 0; index < tileCount; index++)
        {
            var tile = Tile.LoadFromStream(stream);
            tileset.ColorSpace ??= tile.Bitmap.ColorSpace;
            tileset.Tiles.Add(tile);
        }
        return tileset;
    }

    /// <summary>
    /// Loads a tileset from the specified file.
    /// </summary>
    /// <param name="filename">The name of the tileset file.</param>
    /// <returns>A new <see cref="ITileset" /> loaded from the specified file.</returns>
    public static ITileset LoadFromFile(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        return LoadFromStream(stream);
    }

    /// <summary>
    /// Saves the tileset to the specified stream using the specified format and quality.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="format">The format of the tile images.</param>
    /// <param name="quality">The quality of the tile images.</param>
    public void SaveToStream(Stream stream, SKEncodedImageFormat format, int quality = 100)
    {
        WriteHeader(stream);
        foreach (var tile in Tiles)
            tile.SaveToStream(stream, format, quality);
    }

    /// <summary>
    /// Saves the tileset to the specified stream as a full quality PNG.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    public void SaveToStream(Stream stream)
    {
        SaveToStream(stream, SKEncodedImageFormat.Png);
    }

    /// <summary>
    /// Saves the tileset to the specified file using the specified format and quality.
    /// </summary>
    /// <param name="filename">The name of the tileset file.</param>
    /// <param name="format">The format of the tile images.</param>
    /// <param name="quality">The quality of the tile images.</param>
    public void SaveToFile(string filename, SKEncodedImageFormat format, int quality = 100)
    {
        using var stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
        SaveToStream(stream, format, quality);
    }

    /// <summary>
    /// Saves the tileset to the specified file as a full quality PNG.
    /// </summary>
    /// <param name="filename">The name of the tileset file.</param>
    public void SaveToFile(string filename)
    {
        SaveToFile(filename, SKEncodedImageFormat.Png);
    }
}
