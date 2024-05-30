using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEdit.ViewModels;

public class ImageEditorViewModel(WriteableBitmap bitmap) : IImageEditorViewModel
{
    public WriteableBitmap Bitmap { get; } = bitmap;

    public ImageEditorViewModel() : this(
        new WriteableBitmap(
            new PixelSize(32, 32),
            new Vector(96, 96),
            PixelFormat.Rgba8888,
            AlphaFormat.Unpremul)
        )
    {
    }
}
