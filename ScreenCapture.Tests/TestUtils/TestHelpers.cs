using System.Drawing;

namespace ScreenCapture.Tests.TestUtils
{
    public class TestHelpers
    {
        public static Bitmap CreateTestBitmap()
        {
            // Create a new bitmap with a size of 100x100 pixels
            Bitmap bmp = new Bitmap(100, 100);

            // Fill the entire image with a white background
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }

            // Draw a red square in the center of the image
            int squareSize = 20;
            int squareX = (bmp.Width - squareSize) / 2;
            int squareY = (bmp.Height - squareSize) / 2;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Red, squareX, squareY, squareSize, squareSize);
            }

            return bmp;
        }
    }
}
