using Avalonia.Media.Imaging;

namespace TileEdit.ViewModels;
public interface IImageEditorViewModel
{
    WriteableBitmap Bitmap { get; }
}