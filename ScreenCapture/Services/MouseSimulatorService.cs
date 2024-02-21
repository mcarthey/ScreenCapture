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
            x += windowRect.Left;
            y += windowRect.Top;
        }

        // Normalize the coordinates
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;
        double normalizedX = (x / screenWidth) * 65535;
        double normalizedY = (y / screenHeight) * 65535;

        // Simulate the click
        var inputSimulator = new InputSimulator();
        inputSimulator.Mouse.MoveMouseTo(normalizedX, normalizedY);
        inputSimulator.Mouse.LeftButtonClick();
    }


}
