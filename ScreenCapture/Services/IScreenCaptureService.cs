using System.Drawing;

namespace ScreenCapture.Services;

public interface IScreenCaptureService
{
    Bitmap CaptureScreen(Rectangle area);
    Bitmap CaptureWindow(string windowTitle, out IntPtr windowHandle);
}
