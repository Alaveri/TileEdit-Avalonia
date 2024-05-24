using Alaveri.Avalonia;
using Alaveri.Configuration;
using Avalonia.Controls;
using System.Collections.Generic;

namespace TileEdit.Common;

/// <summary>
/// Contains user-specific configuration settings.
/// </summary>
public interface IUserConfiguration : IConfiguration
{
    /// <summary>
    /// The states of application windows.
    /// </summary>
    IDictionary<string, StoredWindowState> WindowStates { get; }

    /// <summary>
    /// The width of the left pane.
    /// </summary>
    double LeftPanelWidth { get; set; }

    /// <summary>
    /// The width of the right pane.
    /// </summary>
    double RightPanelWidth { get; set; }

    /// <summary>
    /// Stores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to store.</param>
    void RestoreWindowState(string windowName, Window window);

    /// <summary>
    /// Restores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to restore.</param>
    void StoreWindowState(string windowName, Window window);
}