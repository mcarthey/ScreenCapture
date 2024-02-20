using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ScreenCapture.Extensions;

public static class BitmapExtensions
{
    public static unsafe Image<Bgr, byte> ToImage(this Bitmap bitmap)
    {
        var image = new Image<Bgr, byte>(bitmap.Width, bitmap.Height);
        var data = image.Data;
        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        unsafe
        {
            byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int offset = y * bitmapData.Stride + x * 3;
                    data[y, x, 0] = ptr[offset + 0]; // B
                    data[y, x, 1] = ptr[offset + 1]; // G
                    data[y, x, 2] = ptr[offset + 2]; // R
                }
            }
        }

        bitmap.UnlockBits(bitmapData);
        return image;
    }
}