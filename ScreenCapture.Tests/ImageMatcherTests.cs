using System.Drawing;
using ScreenCapture.Services;

public class ImageMatcherTests : IClassFixture<TestImageFixture>, IDisposable
{
    private readonly TestImageFixture _fixture;

    public ImageMatcherTests(TestImageFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.Dispose();
    }

    [Fact]
    public void FindTargetInSource_ShouldReturnLocation_WhenImageIsPresent()
    {
        // Arrange
        Bitmap sourceImage = _fixture.CreateTestBitmap();
        Bitmap targetImage = _fixture.CreateTargetBitmap(Brushes.Red);

        Point expectedLocation = new Point(40, 40); // Adjust based on your images

        // Act
        Point? actualLocation = ImageMatcherService.FindTargetInSource(sourceImage, targetImage);

        // Assert
        Assert.NotNull(actualLocation);
        Assert.Equal(expectedLocation, actualLocation.Value);
    }

    [Fact]
    public void FindTargetInSource_ShouldReturnNull_WhenImageIsNotPresent()
    {
        // Arrange
        Bitmap sourceImage = _fixture.CreateTestBitmap();
        Bitmap targetImage = _fixture.CreateTargetBitmap(Brushes.Blue);

        // Act
        Point? actualLocation = ImageMatcherService.FindTargetInSource(sourceImage, targetImage);

        // Assert
        Assert.Null(actualLocation);
    }
}