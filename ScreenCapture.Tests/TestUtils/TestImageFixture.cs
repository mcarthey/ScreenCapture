using System.Drawing;
using ScreenCapture.Tests.TestUtils;

public class TestImageFixture : IDisposable
{
    private const string SourceImagePath = "source.bmp";
    private const string TargetImagePath = "target.bmp";

    public Bitmap CreateTestBitmap()
    {
        Bitmap sourceImage = TestHelpers.CreateTestBitmap();
        sourceImage.Save(SourceImagePath);
        return sourceImage;
    }

    public Bitmap CreateTargetBitmap(Brush brush)
    {
        Bitmap targetImage = new Bitmap(20, 20); // Create a smaller image with a specific pattern
        using (Graphics g = Graphics.FromImage(targetImage))
        {
            g.FillRectangle(brush, 0, 0, 20, 20);
        }
        targetImage.Save(TargetImagePath);
        return targetImage;
    }

    public void Dispose()
    {
        if (File.Exists(SourceImagePath))
        {
            File.Delete(SourceImagePath);
        }

        if (File.Exists(TargetImagePath))
        {
            File.Delete(TargetImagePath);
        }
    }
}
