using Alaveri.Avalonia;
using Alaveri.Configuration;
using Avalonia.Controls;
using System;
using System.Collections.Generic;

namespace TileEdit.Common;

/// <summary>
/// Contains user-specific configuration settings.
/// </summary>
public sealed class UserConfiguration : Configuration, IUserConfiguration
{
    /// <summary>
    /// The states of application windows.
    /// </summary>
    public IDictionary<string, StoredWindowState> WindowStates { get; } = new Dictionary<string, StoredWindowState>(StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// The width of the left pane.
    /// </summary>
    public double LeftPanelWidth { get; set; } = 200;

    /// <summary>
    /// The width of the right pane.
    /// </summary>
    public double RightPanelWidth { get; set; } = 200;

    /// <summary>
    /// Restores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to restore.</param>
    public void RestoreWindowState(string windowName, Window window)
    {
        if (!WindowStates.TryGetValue(windowName, out var state))
        {
            state = new StoredWindowState(1024, 768);
            WindowStates.Add(windowName, state);
        }
        state.RestoreWindowState(window);
    }

    /// <summary>
    /// Stores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to store.</param>
    public void StoreWindowState(string windowName, Window window)
    {
        if (!WindowStates.TryGetValue(windowName, out var state))
        {
            state = new StoredWindowState();
            WindowStates.Add(windowName, state);
        }
        state.StoreWindowState(window);
    }
}
