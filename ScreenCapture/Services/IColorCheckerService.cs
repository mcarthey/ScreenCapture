using System.Drawing;

namespace ScreenCapture.Services;

public interface IColorCheckerService
{
    bool IsColorPresent(Bitmap bitmap, Color color);
}
