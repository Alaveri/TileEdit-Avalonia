using Microsoft.CodeAnalysis;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEdit.Models;

public enum TilesetDimensions
{
    Fixed,
    Variable
}

public sealed class Tileset
{
    public TilesetDimensions Dimensions { get; }

    protected IList<Tile> Tiles { get; } = [];

    public SKColorSpace ColorSpace { get; }

    public SKAlphaType AlphaType { get; }

    public SKColorType ColorType { get; }

    public int TileWidth { get; }

    public int TileHeight { get; }

    public int TileCount => Tiles.Count;

    public Tileset(int width, int height, SKColorType colorType, SKAlphaType alphaType = SKAlphaType.Premul, SKColorSpace? colorSpace = null)
    {
        colorSpace ??= SKColorSpace.CreateSrgb();
        ColorSpace = colorSpace;
        ColorType = colorType;
        Dimensions = TilesetDimensions.Fixed;
        TileWidth = width;
        TileHeight = height;
        AlphaType = alphaType;
    }

    public Tileset(SKColorType colorType, SKAlphaType alphaType = SKAlphaType.Premul, SKColorSpace? colorSpace = null)
    {
        colorSpace ??= SKColorSpace.CreateSrgb();
        ColorSpace = colorSpace;
        ColorType = colorType;
        Dimensions = TilesetDimensions.Variable;
        TileWidth = 0;
        TileHeight = 0;
        AlphaType = alphaType;
    }

    public (Tile Tile, int TileIndex) AddTile(SKColorType? colorType = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.Fixed)
            throw new InvalidOperationException("Cannot add a fixed width tile to a variable tileset.");
        colorType ??= ColorType;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(TileWidth, TileHeight, colorType.Value, alphaType.Value, colorSpace);
        Tiles.Add(tile);
        return (tile, Tiles.Count - 1);
    }

    public Tile this[int index] => Tiles[index];

    public (Tile Tile, int TileIndex) AddTile(int width, int height, SKColorType? colorType = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.Variable)
            throw new InvalidOperationException("Cannot add a variable width tile to a fixed tileset.");
        colorType ??= ColorType;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(width, height, colorType.Value, alphaType.Value, colorSpace);
        Tiles.Add(tile);
        return (tile, Tiles.Count - 1);
    }

    public void RemoveTile(Tile tile)
    {        
        Tiles.Remove(tile);
    }

    public void ClearTiles()
    {
        Tiles.Clear();
    }

    public void SwapTiles(int index1, int index2)
    {
        (Tiles[index2], Tiles[index1]) = (Tiles[index1], Tiles[index2]);
    }

    public void MoveTile(int fromIndex, int toIndex)
    {
        var tile = Tiles[fromIndex];
        Tiles.RemoveAt(fromIndex);
        Tiles.Insert(toIndex, tile);
    }

    public Tile InsertTile(int index, int width, int height, SKColorType? colorType = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        if (Dimensions != TilesetDimensions.Variable)
            throw new InvalidOperationException("Cannot add a variable width tile to a fixed tileset.");
        colorType ??= ColorType;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(width, height, colorType.Value, alphaType.Value, colorSpace);
        Tiles.Insert(index, tile);
        return tile;
    }

    public Tile InsertTile(int index, SKColorType? colorType = null, SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    { 
        if (Dimensions != TilesetDimensions.Fixed)
            throw new InvalidOperationException("Cannot add a fixed width tile to a variable tileset.");
        colorType ??= ColorType;
        alphaType ??= AlphaType;
        colorSpace ??= ColorSpace;
        var tile = new Tile(TileWidth, TileHeight, colorType.Value, alphaType.Value, colorSpace);
        Tiles.Insert(index, tile);
        return tile;
    }

    public void RemoveTile(int index)
    {
        Tiles.RemoveAt(index);
    }   

    private void WriteHeader(Stream stream, SKEncodedImageFormat format, int quality)
    {
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(TilesetConstants.MajorVersion);
        writer.Write(TilesetConstants.MinorVersion);
        writer.Write((int)Dimensions);
        writer.Write((int)format);
        writer.Write(quality);
        writer.Write(TileWidth);
        writer.Write(TileHeight);
        writer.Write((int)ColorType);
        writer.Write((int)AlphaType);
        writer.Write(Tiles.Count);
    }

    public static Tileset LoadFromStream(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        var majorVersion = reader.ReadInt32();
        var minorVersion = reader.ReadInt32();
        if (majorVersion != TilesetConstants.MajorVersion || minorVersion != TilesetConstants.MinorVersion)
            throw new InvalidOperationException("Invalid tileset version.");
        var dimensions = (TilesetDimensions)reader.ReadInt32();
        var format = (SKEncodedImageFormat)reader.ReadInt32();
        var quality = reader.ReadInt32();
        var width = reader.ReadInt32();
        var height = reader.ReadInt32();
        var colorType = (SKColorType)reader.ReadInt32();
        var alphaType = (SKAlphaType)reader.ReadInt32();
        var colorSpace = SKColorSpace.CreateSrgb();
        var tileset = dimensions == TilesetDimensions.Fixed ? 
            new Tileset(width, height, colorType, alphaType, colorSpace) : 
            new Tileset(colorType, alphaType, colorSpace);
        var tileCount = reader.ReadInt32();
        for (var index = 0; index < tileCount; index++)
        {
            var tile = Tile.LoadFromStream(stream);
            tileset.Tiles.Add(tile);
        }
        return tileset;
    }

    public void SaveToStream(Stream stream, SKEncodedImageFormat format, int quality)
    {
        WriteHeader(stream, format, quality);
        foreach (var tile in Tiles)
            tile.SaveToStream(stream, format, quality);
    }

    public void SaveToStream(Stream stream)
    {
        SaveToStream(stream, SKEncodedImageFormat.Png, 100);
    }

    public void SaveToFile(string filename, SKEncodedImageFormat format, int quality)
    {
        using var stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
        SaveToStream(stream, format, quality);
    }

    public void SaveToFile(string filename)
    {
        SaveToFile(filename, SKEncodedImageFormat.Png, 100);
    }
}
