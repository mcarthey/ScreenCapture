using System.Drawing;

namespace ScreenCapture.Services;

public class ScreenCaptureServiceService : IScreenCaptureService
{
    public Bitmap CaptureScreen(Rectangle area)
    {
        Bitmap screenBitmap = new Bitmap(area.Width, area.Height);
        using (Graphics graphics = Graphics.FromImage(screenBitmap))
        {
            graphics.CopyFromScreen(area.Location, Point.Empty, area.Size);
        }
        return screenBitmap;
    }

    public Bitmap CaptureWindow(string windowTitle)
    {
        nint windowHandle = User32.FindWindow(null, windowTitle);
        if (windowHandle == nint.Zero)
        {
            throw new Exception($"Window not found: {windowTitle}");
        }

        User32.RECT windowRect;
        User32.GetWindowRect(windowHandle, out windowRect);

        Rectangle windowBounds = new Rectangle(windowRect.Left, windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);
        Bitmap windowBitmap = new Bitmap(windowBounds.Width, windowBounds.Height);

        using (Graphics graphics = Graphics.FromImage(windowBitmap))
        {
            graphics.CopyFromScreen(windowBounds.Location, Point.Empty, windowBounds.Size);
        }

        return windowBitmap;
    }
}
