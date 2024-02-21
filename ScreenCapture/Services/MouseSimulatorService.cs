using System.Diagnostics.Metrics;
using WindowsInput;
using System.Windows;

namespace ScreenCapture.Services;

public class MouseSimulatorService : IMouseSimulatorService
{
    public void SimulateClick(int x, int y, IntPtr windowHandle)
    {
        if (windowHandle != IntPtr.Zero)
        {
            // Get the top-left corner of the window
            User32.RECT windowRect;
            User32.GetWindowRect(windowHandle, out windowRect);

            // Adjust the click coordinates to be relative to the window
            // This is necessary because the click coordinates are given in screen coordinates,
            // but the click needs to be performed relative to the window.
            x += windowRect.Left;
            y += windowRect.Top;
        }

        // Normalize the coordinates
        // The InputSimulator library expects coordinates in a normalized format,
        // where (0,0) is the top-left corner of the screen and (65535,65535) is the bottom-right corner.
        // Therefore, we need to convert our screen coordinates to this normalized format.
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;
        double normalizedX = (x / screenWidth) * 65535;
        double normalizedY = (y / screenHeight) * 65535;

        // Simulate the click
        // We first move the mouse to the desired location, and then perform a left button click.
        var inputSimulator = new InputSimulator();
        inputSimulator.Mouse.MoveMouseTo(normalizedX, normalizedY);
        inputSimulator.Mouse.LeftButtonClick();
    }
}
