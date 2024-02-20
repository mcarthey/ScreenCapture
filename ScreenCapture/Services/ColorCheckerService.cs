using System.Drawing;

namespace ScreenCapture.Services;

public class ColorCheckerService : IColorCheckerService
{
    public bool IsColorPresent(Bitmap bitmap, Color color)
    {
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (bitmap.GetPixel(x, y) == color)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
